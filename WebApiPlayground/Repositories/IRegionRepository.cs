using Microsoft.AspNetCore.Mvc;
using WebApiPlayground.Models.Domain;

namespace WebApiPlayground.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllRegionsAsync();
        Task<Region?> GetRegionByIdAsync(Guid id);
        Task<Region> CreateRegionAsync(Region region);
        Task<Region?> UpdateRegionAsync(Guid id, Region region);
        Task<Region?> DeleteRegionByIdAsync(Guid id);
    }
}