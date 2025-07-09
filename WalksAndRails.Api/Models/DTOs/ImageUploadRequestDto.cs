using System.ComponentModel.DataAnnotations;

namespace WalksAndRails.Api.Models.DTOs
{
    public class ImageUploadRequestDto
    {
        [Required]
        public required IFormFile File { get; set; }
        [Required]
        public required string FileName { get; set; }
        public string? FileDescription { get; set; }
    }
}
