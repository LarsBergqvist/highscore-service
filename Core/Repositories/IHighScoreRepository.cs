using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IHighScoreRepository
    {
        Task<IEnumerable<HighScoreList>> GetAllHighScoreLists();
        Task<HighScoreList> GetHighScoreList(string highScoreListId);
        Task<HighScoreList> CreateHighScoreList(HighScoreListInput highScoreListInput);
        Task AddGameResultToHighScoreList(string highScoreListId, GameResult gameResult);
    }
}
