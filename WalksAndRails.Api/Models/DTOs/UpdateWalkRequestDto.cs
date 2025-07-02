using System.ComponentModel.DataAnnotations;

namespace WalksAndRails.Api.Models.DTOs
{
    public class UpdateWalkRequestDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "Name has to be minimum 2 characters")]
        [MaxLength(100, ErrorMessage = "Name has to be maximum 100 characters")]
        public required string Name { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = "Description has to be minimum 10 characters")]
        [MaxLength(1000, ErrorMessage = "Description has to be maximum 1000 characters")]
        public required string Description { get; set; }
        [Required]
        [Range(0, 50, ErrorMessage = "Length must be between 0 and 50 km")]
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }
    }
}
