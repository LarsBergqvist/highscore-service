using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class FakeHighScoreRepository : IHighScoreRepository
    {
        private List<HighScoreList> _highScoreLists;
        public FakeHighScoreRepository()
        {
            _highScoreLists = new List<HighScoreList>();
            _highScoreLists.Add(new HighScoreList(
                Guid.Parse("2252925F-95FB-40EE-9912-039657B6F57D"),
                "Sliding Image Puzzle 3x3 High Score List",
                true, 3
                )
            {
                Results = new List<GameResult> {
                    new GameResult("Pelle", 42),
                    new GameResult("Lisa", 99),
                    new GameResult("Olle", 105)
                },
            });

        }

        public Task AddGameResultToHighScoreList(Guid highScoreListId, GameResult gameResult)
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
                updatedResults = results.Select(c => c).OrderBy(c => c.Result).Take(list.MaxSize).ToList();
            }
            else
            {
                updatedResults = results.Select(c => c).OrderByDescending(c => c.Result).Take(list.MaxSize).ToList();
            }

            list.Results = updatedResults;
            return Task.CompletedTask;
        }

        public Task<HighScoreList> CreateHighScoreList(HighScoreListInput input)
        {
            var id = Guid.NewGuid();
            var highScoreList = new HighScoreList(id, input.Name, input.LowIsBest, input.MaxSize);
            _highScoreLists.Add(highScoreList);
            return Task.FromResult(highScoreList);
        }

        public Task<IEnumerable<HighScoreList>> GetAllHighScoreLists()
        {
            var result = _highScoreLists.AsEnumerable();
            return Task.FromResult(result);
        }

        public Task<HighScoreList> GetHighScoreList(Guid highScoreListId)
        {
            return Task.FromResult(_highScoreLists.FirstOrDefault(h => h.Id == highScoreListId));
        }
    }
}
