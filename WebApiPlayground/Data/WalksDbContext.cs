using Microsoft.EntityFrameworkCore;
using WebApiPlayground.Models.Domain;

namespace WebApiPlayground.Data
{
    public class WalksDbContext: DbContext
    {
        private readonly IConfiguration _configuration;

        public WalksDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("WalksConnectionString"));
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
    }
}
