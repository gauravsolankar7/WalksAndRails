using System.ComponentModel.DataAnnotations;

namespace WalksAndRails.Api.Models.DTOs
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage ="Code has to be minimum 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be maximum 3 characters")]
        public required string Code { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Name has to be minimum 2 characters")]
        [MaxLength(100, ErrorMessage = "Name has to be maximum 100 characters")]
        public required string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
