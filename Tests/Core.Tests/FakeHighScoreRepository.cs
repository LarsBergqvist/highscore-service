using Core.Models;
using Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Helpers;

namespace Core.Tests
{
    public class FakeHighScoreRepository : IHighScoreRepository
    {
        private readonly List<HighScoreListReadModel> _highScoreLists;
        private readonly IGameResultHelper _gameResultHelper;
        public FakeHighScoreRepository(IGameResultHelper gameResultHelper)
        {
            _gameResultHelper = gameResultHelper;
            _highScoreLists = new List<HighScoreListReadModel>();
            _highScoreLists.Add(new HighScoreListReadModel(
                "1",
                "My game",
                true, "Moves", 10
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

            var newResults =
                _gameResultHelper.GetSortedListWithNewResult(gameResult,
                            list.Results,
                            list.LowIsBest,
                            list.MaxSize);

            list.Results = newResults;
            return Task.CompletedTask;
        }

        public Task<HighScoreListReadModel> CreateHighScoreList(HighScoreListWriteModel input)
        {
            var id = (_highScoreLists.Count + 1).ToString();
            var highScoreList = new HighScoreListReadModel(id, input.Name, input.LowIsBest, input.Unit, input.MaxSize);
            _highScoreLists.Add(highScoreList);
            return Task.FromResult(highScoreList);
        }

        public Task<IEnumerable<HighScoreListReadModel>> GetAllHighScoreLists()
        {
            var result = _highScoreLists.AsEnumerable();
            return Task.FromResult(result);
        }

        public Task<HighScoreListReadModel> GetHighScoreList(string highScoreListId)
        {
            return Task.FromResult(_highScoreLists.FirstOrDefault(h => h.Id == highScoreListId));
        }
    }
}
