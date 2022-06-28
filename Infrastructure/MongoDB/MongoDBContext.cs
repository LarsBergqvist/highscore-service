using Core.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.MongoDB;

public interface IMongoDBContext
{
    IMongoCollection<T> GetCollection<T>(string name);
}

public class MongoDBContext : IMongoDBContext
{
    private readonly IMongoDatabase _db;
    public MongoDBContext(IOptions<RepositorySettings> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        _db = mongoClient.GetDatabase(options.Value.HighScoresDBName);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _db.GetCollection<T>(name);
    }
}