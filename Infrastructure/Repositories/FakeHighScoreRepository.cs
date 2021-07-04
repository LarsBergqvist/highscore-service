using Core.Models;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class FakeHighScoreRepository : IHighScoreRepository
    {
        private List<HighScoreList> _highScoreLists;
        public FakeHighScoreRepository()
        {
            _highScoreLists = new List<HighScoreList>();
            _highScoreLists.Add(new HighScoreList(
                "1",
                "Sliding Image Puzzle 3x3 High Score List",
                true, "Moves", 3
                )
            {
                Results = new List<GameResult> {
                    new GameResult("Pelle", 42),
                    new GameResult("Lisa", 99),
                    new GameResult("Olle", 105)
                },
            });

        }

        public Task AddGameResultToHighScoreList(string highScoreListId, GameResult gameResult)
        {
            var list = _highScoreLists.FirstOrDefault(h => h.Id == highScoreListId);
            if (list == null)
            {
                return Task.CompletedTask;
            }

            var results = list.Results;
            list.Results.Add(gameResult);

            IList<GameResult> updatedResults;
            if (list.LowIsBest)
            {
                updatedResults = results.Select(c => c).OrderBy(c => c.Score).Take(list.MaxSize).ToList();
            }
            else
            {
                updatedResults = results.Select(c => c).OrderByDescending(c => c.Score).Take(list.MaxSize).ToList();
            }

            list.Results = updatedResults;
            return Task.CompletedTask;
        }

        public Task<HighScoreList> CreateHighScoreList(HighScoreListInput input)
        {
            var id = (_highScoreLists.Count + 1).ToString();
            var highScoreList = new HighScoreList(id, input.Name, input.LowIsBest, input.Unit, input.MaxSize);
            _highScoreLists.Add(highScoreList);
            return Task.FromResult(highScoreList);
        }

        public Task<IEnumerable<HighScoreList>> GetAllHighScoreLists()
        {
            var result = _highScoreLists.AsEnumerable();
            return Task.FromResult(result);
        }

        public Task<HighScoreList> GetHighScoreList(string highScoreListId)
        {
            return Task.FromResult(_highScoreLists.FirstOrDefault(h => h.Id == highScoreListId));
        }
    }
}
