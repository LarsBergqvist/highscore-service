using Core.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Infrastructure.MongoDB;

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

    public HighScoreListReadModel ToReadModel()
    {
        var readModel = new HighScoreListReadModel(Id.ToString(), Name, LowIsBest, Unit, MaxSize);
        readModel.Results = Results;
        return readModel;
    }

    [BsonId]
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public IList<GameResult> Results { get; set; }
    public bool LowIsBest { get; set; }
    public int MaxSize { get; set; }
    public string Unit { get; set; }
}