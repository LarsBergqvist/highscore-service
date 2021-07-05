using Core.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.MongoDB
{

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
