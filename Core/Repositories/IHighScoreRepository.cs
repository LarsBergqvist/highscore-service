using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Repositories;

public interface IHighScoreRepository
{
    Task<IEnumerable<HighScoreListReadModel>> GetAllHighScoreLists();
    Task<HighScoreListReadModel> GetHighScoreList(string highScoreListId);
    Task<HighScoreListReadModel> CreateHighScoreList(HighScoreListWriteModel highScoreListInput);
    Task AddGameResultToHighScoreList(string highScoreListId, GameResult gameResult);
}