using Microsoft.EntityFrameworkCore;
using WebApiPlayground.Data;
using WebApiPlayground.Models.Domain;

namespace WebApiPlayground.Repositories
{
    public class SqlWalkRepository : IWalkRepository
    {
        private readonly WalksDbContext _dbContext;
        public SqlWalkRepository(WalksDbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null)
        {
            var walks = _dbContext.Walks
                .Include(x => x.Difficulty)
                .Include(x => x.Region)
                .AsQueryable();

            // Filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                switch (filterOn.ToLower())
                {
                    case "name":
                        walks = walks.Where(x => x.Name.Contains(filterQuery));
                        break;
                    case "length":
                        walks = walks.Where(x => x.LengthInKm == Double.Parse(filterQuery));
                        break;
                    default:
                        break;
                }
            }

            return await walks.ToListAsync();
        }

        public async Task<Walk?> GetWalkByIdAsync(Guid id)
        {
            return await _dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Walk> CreateWalkAsync(Walk walk)
        {
            _dbContext.Walks.Add(walk);
            await _dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> UpdateWalkAsync(Guid id, Walk walk)
        {
            var existingWalk = await _dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
                return null;

            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.Region = walk.Region;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.DifficultyId = walk.DifficultyId;

            await _dbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<Walk?> DeleteWalkByIdAsync(Guid id)
        {
            var walk = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walk == null)
                return null;

            _dbContext.Walks.Remove(walk);
            await _dbContext.SaveChangesAsync();
            return walk;
        }
    }
}
