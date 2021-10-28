using System.Collections.Generic;
using Core.Models;

namespace Core.Helpers
{
    public interface IGameResultHelper
    {
        IList<GameResult> GetSortedListWithNewResult(GameResult newResult, IList<GameResult> oldResults, bool lowIsBest, int maxSize);
    }
}