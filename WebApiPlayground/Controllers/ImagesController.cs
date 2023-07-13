using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApiPlayground.CustomActionFilters;
using WebApiPlayground.Models.Domain;
using WebApiPlayground.Models.DTOs;
using WebApiPlayground.Repositories;

namespace WebApiPlayground.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        public IImageRepository _imageRepository { get; }

        public ImagesController(IMapper mapper, IImageRepository imageRepository) {
            _mapper = mapper;
            _imageRepository = imageRepository;
        }

        [HttpPost("Upload")]
        [ValidateModel]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);
            var imageDomain = _mapper.Map<Image>(request);
            await _imageRepository.Upload(imageDomain);
            return Ok(imageDomain);
        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
                ModelState.AddModelError("file", "Unsupported file extension.");

            if (request.File.Length > 10485670)
                ModelState.AddModelError("file", "File size more than 10MB, please upload smaller size file.");
        }
    }
}
