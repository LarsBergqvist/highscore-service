using Core.Models;
using Core.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Infrastructure.MongoDB
{
    public class HighScoreListDBModel
    {

        public HighScoreListDBModel(ObjectId id, string name, bool lowIsBest, string unit, int maxSize)
        {
            Id = id;
            Name = name;
            Results = new List<GameResult>();
            LowIsBest = lowIsBest;
            MaxSize = maxSize;
            Unit = unit;
        }

        [BsonId]
        public ObjectId Id { get; set; }
//        public string Id { get; set; }
        public string Name { get; set; }
        public IList<GameResult> Results { get; set; }
        public bool LowIsBest { get; set; }
        public int MaxSize { get; set; }
        public string Unit { get; set; }
    }

    public interface IMongoDBContext
    {
        IMongoCollection<HighScoreListDBModel> GetCollection<HighScoreListDBModel>(string name);
    }

    public class MongoDBContext : IMongoDBContext
    {
        private readonly IMongoDatabase _db;
        private readonly MongoClient _mongoClient;
        public MongoDBContext(IOptions<RepositorySettings> options)
        {
            _mongoClient = new MongoClient(options.Value.ConnectionString);
            _db = _mongoClient.GetDatabase(options.Value.HighScoresDBName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}
