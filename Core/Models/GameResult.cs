using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class GameResult
    {
        public GameResult(string userName, int result)
        {
            UserName = userName;
            Result = result;
        }

        [Required]
        public string UserName { get; init; }
        [Required]
        public int Result { get; init; }
    }
}
