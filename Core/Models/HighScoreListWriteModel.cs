using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class HighScoreListWriteModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The {0} value must be between 3 and 50 characters.")]
        public string Name { get; init; }
        [Required]
        public bool LowIsBest { get; init; }
        [Required]
        public string Unit { get; init; }
        [Required]
        [Range(1, 25)]
        public int MaxSize { get; init; }
    }
}
