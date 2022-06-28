using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models;

public class GameResult
{
    public GameResult() { }

    public GameResult(string userName, int score)
    {
        UserName = userName;
        Score = score;
    }

    public GameResult(GameResult source)
    {
        UserName = source.UserName;
        Score = source.Score;
        UtcDateTime = source.UtcDateTime;
        Id = source.Id;            
    }

    [Required]
    [StringLength(25, MinimumLength = 3, ErrorMessage = "The {0} value must be between 3 and 25 characters.")]
    public string UserName { get; init; }
    [Required]
    public int Score { get; init; }
    public DateTime UtcDateTime { get; init; }
    public string Id { get; set; }
}