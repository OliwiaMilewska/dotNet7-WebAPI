using WebApiPlayground.Data;
using WebApiPlayground.Models.Domain;

namespace WebApiPlayground.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _env;
        public IHttpContextAccessor _accessor { get; }
        private readonly WalksDbContext _dbContext;

        public LocalImageRepository(IWebHostEnvironment env, IHttpContextAccessor accessor, WalksDbContext dbContext)
        {
            _env = env;
            _accessor = accessor;
            _dbContext = dbContext;
        }

        public async Task<Image> Upload(Image image)
        {
            // Get local path of folder Images
            var localFilePath = Path.Combine(_env.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");

            // Upload image to local path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // https://localhost:1234/images/image.jpg
            var urlFilePath = $"{_accessor.HttpContext.Request.Scheme}://{_accessor.HttpContext.Request.Host}{_accessor.HttpContext.Request.PathBase}/images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            // Add images to Image Table
            await _dbContext.Images.AddAsync(image);
            await _dbContext.SaveChangesAsync();

            return image;
        }
    }
}
