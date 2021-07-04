using Core.Models;
using Core.Repositories;
using Core.Settings;
using Infrastructure.MongoDB;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{


    public class HighScoreRepository : IHighScoreRepository
    {
        private readonly IMongoDBContext _mongoDBContext;
        private readonly RepositorySettings _settings;
        public HighScoreRepository(IOptions<RepositorySettings> options, IMongoDBContext mongoDBContext)
        {
            _settings = options.Value;
            _mongoDBContext = mongoDBContext;
        }

        // 60e183b7ef1e0c7c557fd690
        public async Task AddGameResultToHighScoreList(string highScoreListId, GameResult gameResult)
        {
            var list = await GetHighScoreList(highScoreListId);
            if (list == null)
            {
                return;
            }

            var results = list.Results;
            list.Results.Add(gameResult);

            IList<GameResult> updatedResults;
            if (list.LowIsBest)
            {
                updatedResults = results.Select(c => c).OrderBy(c => c.Score).Take(list.MaxSize).ToList();
            }
            else
            {
                updatedResults = results.Select(c => c).OrderByDescending(c => c.Score).Take(list.MaxSize).ToList();
            }

            list.Results = updatedResults;

            try
            {
                var dbModel = new HighScoreListDBModel(new ObjectId(list.Id), list.Name, list.LowIsBest, list.Unit, list.MaxSize);
                dbModel.Results = list.Results;
                var collection = GetCollection();
                var filter = Builders<HighScoreListDBModel>.Filter.Eq("_id", dbModel.Id);
                var replaceOptions = new ReplaceOptions
                {
                    IsUpsert = true
                };
                await collection.ReplaceOneAsync(filter, dbModel, replaceOptions);
            }
            catch (Exception e)
            {
//                _logger.LogError(e.ToString());
                throw (e);
            }
        }

        public async Task<HighScoreList> CreateHighScoreList(HighScoreListInput input)
        {
            var collection = GetCollection();
            var objId = ObjectId.GenerateNewId();
            var dbModel = new HighScoreListDBModel(objId, input.Name, input.LowIsBest, input.Unit, input.MaxSize);
                await collection.InsertOneAsync(dbModel);

            var highScoreList = new HighScoreList(objId.ToString(), input.Name, input.LowIsBest, input.Unit, input.MaxSize);

            return highScoreList;
        }

        public async Task<IEnumerable<HighScoreList>> GetAllHighScoreLists()
        {
            try
            {
                var collection = GetCollection();
                var filter = Builders<HighScoreListDBModel>.Filter.Ne("Type", "Övrigt");
                var asyncCursor = await collection.FindAsync<HighScoreListDBModel>(filter);
                var result = asyncCursor.ToEnumerable<HighScoreListDBModel>();
                var list = new List<HighScoreList>();
                foreach(var res in result)
                {
                    var l = new HighScoreList(res.Id.ToString(), res.Name, res.LowIsBest,res.Unit, res.MaxSize);
                    l.Results = res.Results;
                    list.Add(l);
                }
                return list;
            }
            catch (Exception e)
            {
//                _logger.LogError(e.ToString());
                throw (e);
            }
        }

        public async Task<HighScoreList> GetHighScoreList(string highScoreListId)
        {
            try
            {
                var collection = GetCollection();
                var filter = Builders<HighScoreListDBModel>.Filter.Eq("_id", new ObjectId(highScoreListId));
                var asyncCursor = await collection.FindAsync<HighScoreListDBModel>(filter);
                var result = asyncCursor.ToList();
                if (result.Count == 0)
                {
                    return null;
                }
                var res = result[0];
                var l = new HighScoreList(res.Id.ToString(), res.Name, res.LowIsBest, res.Unit, res.MaxSize);
                l.Results = res.Results;
                return l;
            }
            catch (Exception e)
            {
//                _logger.LogError(e.ToString());
                throw (e);
            }
        }

        private IMongoCollection<HighScoreListDBModel> GetCollection()
        {
            return _mongoDBContext.GetCollection<HighScoreListDBModel>(_settings.HighScoresCollectionName);
        }
    }
}
