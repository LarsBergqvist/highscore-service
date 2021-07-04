using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models
{
    public class HighScoreList
    {
        public HighScoreList(Guid id, string name, bool lowIsBest, int maxSize)
        {
            Id = id;
            Name = name;
            Results = new List<GameResult>();
            LowIsBest = lowIsBest;
            MaxSize = maxSize;
        }

        public Guid Id { get; }
        public string Name { get; }
        public IList<GameResult> Results { get; set; }
        public bool LowIsBest { get; }
        public int MaxSize { get; }

    }
}
