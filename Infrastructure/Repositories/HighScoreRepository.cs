using Core.Models;
using Core.Repositories;
using Core.Settings;
using Infrastructure.MongoDB;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<HighScoreRepository> _logger;
        public HighScoreRepository(ILogger<HighScoreRepository> logger, IOptions<RepositorySettings> options, IMongoDBContext mongoDBContext)
        {
            _logger = logger;
            _settings = options.Value;
            _mongoDBContext = mongoDBContext;
        }

        public async Task AddGameResultToHighScoreList(string highScoreListId, GameResult gameResult)
        {
            var list = await GetHighScoreList(highScoreListId);
            if (list == null)
            {
                return;
            }

            gameResult.UtcDateTime = DateTime.Now.ToUniversalTime();
            gameResult.Id ??= Guid.NewGuid().ToString();

            //
            // Add the new result and create a new sorted result list limited to MaxSize
            //
            var results = list.Results;
            list.Results.Add(gameResult);

            IList<GameResult> updatedResults;
            if (list.LowIsBest)
            {
                updatedResults = results.Select(c => c).OrderBy(c => c.Score).ThenByDescending(c => c.UtcDateTime).Take(list.MaxSize).ToList();
            }
            else
            {
                updatedResults = results.Select(c => c).OrderByDescending(c => c.Score).ThenByDescending(c => c.UtcDateTime).Take(list.MaxSize).ToList();
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
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<HighScoreListReadModel> CreateHighScoreList(HighScoreListWriteModel input)
        {
            var collection = GetCollection();
            var objId = ObjectId.GenerateNewId();
            var dbModel = new HighScoreListDBModel(objId, input.Name, input.LowIsBest, input.Unit, input.MaxSize);
                await collection.InsertOneAsync(dbModel);

            var highScoreList = new HighScoreListReadModel(objId.ToString(), input.Name, input.LowIsBest, input.Unit, input.MaxSize);

            return highScoreList;
        }

        public async Task<IEnumerable<HighScoreListReadModel>> GetAllHighScoreLists()
        {
            try
            {
                var collection = GetCollection();
                var filter = Builders<HighScoreListDBModel>.Filter.Ne("Type", "Övrigt");
                var asyncCursor = await collection.FindAsync<HighScoreListDBModel>(filter);
                var result = asyncCursor.ToEnumerable();
                var list = new List<HighScoreListReadModel>();
                foreach(var res in result)
                {
                    list.Add(res.ToReadModel());
                }
                return list;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public async Task<HighScoreListReadModel> GetHighScoreList(string highScoreListId)
        {
            if (!ObjectId.TryParse(highScoreListId, out ObjectId filterObjectId))
            {
                return null;
            }
            try
            {
                var collection = GetCollection();
                var filter = Builders<HighScoreListDBModel>.Filter.Eq("_id", filterObjectId);
                var asyncCursor = await collection.FindAsync<HighScoreListDBModel>(filter);
                var result = asyncCursor.ToList();
                if (result.Count == 0)
                {
                    return null;
                }
                return result[0].ToReadModel();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        private IMongoCollection<HighScoreListDBModel> GetCollection()
        {
            return _mongoDBContext.GetCollection<HighScoreListDBModel>(_settings.HighScoresCollectionName);
        }
    }
}
