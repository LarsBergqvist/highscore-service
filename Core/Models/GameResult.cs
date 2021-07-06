using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class GameResult
    {
        public GameResult(string userName, int score)
        {
            UserName = userName;
            Score = score;
        }

        [Required]
        public string UserName { get; init; }
        [Required]
        public int Score { get; init; }
        public DateTime UtcDateTime { get; set; }
        public string Id { get; set; }
    }
}
