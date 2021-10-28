using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;

namespace Core.Helpers
{
    public class GameResultHelper : IGameResultHelper
    {
        public IList<GameResult> GetSortedListWithNewResult(GameResult newResult, IList<GameResult> oldResults, bool lowIsBest, int maxSize)
        {
            //
            // Add the new result and create a new sorted result list limited to MaxSize
            //

            var clonedResult = new GameResult(newResult)
            {
                UtcDateTime = DateTime.Now.ToUniversalTime()
            };
            clonedResult.Id ??= Guid.NewGuid().ToString();

            var newList = new List<GameResult>();
            foreach (var item in oldResults)
            {
                var clone = new GameResult(item);
                newList.Add(clone);
            }

            newList.Add(clonedResult);
            newList = lowIsBest ?
                      newList.Select(c => c).OrderBy(c => c.Score).ThenByDescending(c => c.UtcDateTime).Take(maxSize).ToList() :
                      newList.Select(c => c).OrderByDescending(c => c.Score).ThenByDescending(c => c.UtcDateTime).Take(maxSize).ToList();

            return newList;
        }
    }
}
