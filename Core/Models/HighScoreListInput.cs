using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class HighScoreListInput
    {
        [Required]
        public string Name { get; init; }
        [Required]
        public bool LowIsBest { get; init; }
        [Required]
        public string Unit { get; init; }
        [Required]
        [Range(1, 10)]
        public int MaxSize { get; init; }
    }
}
