using System.Collections.Generic;

namespace Core.Models
{
    public class HighScoreListReadModel
    {
        public HighScoreListReadModel(string id, string name, bool lowIsBest, string unit, int maxSize)
        {
            Id = id;
            Name = name;
            Results = new List<GameResult>();
            LowIsBest = lowIsBest;
            MaxSize = maxSize;
            Unit = unit;
        }

        public string Id { get; }
        public string Name { get; }
        public IList<GameResult> Results { get; set; }
        public bool LowIsBest { get; }
        public int MaxSize { get; }
        public string Unit { get; }

    }
}
