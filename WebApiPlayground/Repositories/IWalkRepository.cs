using Microsoft.AspNetCore.Mvc;
using WebApiPlayground.Models.Domain;

namespace WebApiPlayground.Repositories
{
    public interface IWalkRepository
    {
        Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 1000);
        Task<Walk?> GetWalkByIdAsync(Guid id);
        Task<Walk> CreateWalkAsync(Walk walk);
        Task<Walk?> UpdateWalkAsync(Guid id, Walk walk);
        Task<Walk?> DeleteWalkByIdAsync(Guid id);
    }
}