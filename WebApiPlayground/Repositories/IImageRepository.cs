using WebApiPlayground.Models.Domain;

namespace WebApiPlayground.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
