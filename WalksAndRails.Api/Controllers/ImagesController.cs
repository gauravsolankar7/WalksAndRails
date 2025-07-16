using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalksAndRails.Api.Models.Domain;
using WalksAndRails.Api.Models.DTOs;
using WalksAndRails.Api.Repositories;

namespace WalksAndRails.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        //POST: api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        [Authorize(Roles="Writer")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);
            if(ModelState.IsValid)
            {
                //convert dto to domain model
                var imageModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName).ToLower(),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription
                };
                // Use repository to upload image
                await imageRepository.Upload(imageModel);
                return Ok(imageModel); 

            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png"};
            if (allowedExtensions.Contains(Path.GetExtension(request.File.FileName).ToLower()) == false)
            {
                ModelState.AddModelError("File", "Unsupported file extension");
            }
            if(request.File.Length > 10 * 1024 * 1024) // 10 MB
            {
                ModelState.AddModelError("File", "File size exceeds the maximum limit of 10 MB");
            }
        }
    }
}
