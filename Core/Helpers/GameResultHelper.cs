using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;

namespace Core.Helpers;

public class GameResultHelper : IGameResultHelper
{
    public IList<GameResult> GetSortedListWithNewResult(GameResult newResult, IEnumerable<GameResult> oldResults, bool lowIsBest, int maxSize)
    {
        //
        // Add the new result and create a new sorted result list limited to MaxSize
        //

        var clonedResult = new GameResult(newResult)
        {
            UtcDateTime = DateTime.Now.ToUniversalTime()
        };
        clonedResult.Id ??= Guid.NewGuid().ToString();

        var newList = oldResults.Select(item => new GameResult(item)).ToList();

        newList.Add(clonedResult);
        newList = lowIsBest ?
            newList.Select(c => c).OrderBy(c => c.Score).ThenByDescending(c => c.UtcDateTime).Take(maxSize).ToList() :
            newList.Select(c => c).OrderByDescending(c => c.Score).ThenByDescending(c => c.UtcDateTime).Take(maxSize).ToList();

        return newList;
    }
    
    public ResultListPosition GetProposedPositionInList(int score, IEnumerable<GameResult> oldResults, bool lowIsBest, int maxSize)
    {
        //
        // Add the new result and create a new sorted result list limited to MaxSize
        //

        var clonedResult = new GameResult("", score)
        {
            Id = Guid.NewGuid().ToString(),
            UtcDateTime = DateTime.Now.ToUniversalTime()
        };

        var newList = oldResults.Select(item => new GameResult(item)).ToList();

        newList.Add(clonedResult);
        newList = lowIsBest ?
            newList.Select(c => c).OrderBy(c => c.Score).ThenByDescending(c => c.UtcDateTime).Take(maxSize).ToList() :
            newList.Select(c => c).OrderByDescending(c => c.Score).ThenByDescending(c => c.UtcDateTime).Take(maxSize).ToList();

        var idx = newList.FindIndex(c => c.Id == clonedResult.Id);
        if (idx == -1)
        {
            return new ResultListPosition(-1, ResultStatus.NotInList);
        }

        return new ResultListPosition(idx + 1, ResultStatus.InList);
    }

}