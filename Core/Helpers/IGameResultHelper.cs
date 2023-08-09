using System.Collections.Generic;
using Core.Models;

namespace Core.Helpers;

public interface IGameResultHelper
{
    IList<GameResult> GetSortedListWithNewResult(GameResult newResult, IEnumerable<GameResult> oldResults, bool lowIsBest, int maxSize);

    ResultListPosition GetProposedPositionInList(int score, IEnumerable<GameResult> oldResults,
        bool lowIsBest, int maxSize);
}