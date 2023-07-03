using Microsoft.EntityFrameworkCore;
using WebApiPlayground.Models.Domain;

namespace WebApiPlayground.Data
{
    public class WalksDbContext: DbContext
    {
        public WalksDbContext(DbContextOptions dbContextOptions): base(dbContextOptions)
        {
            
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
    }
}
