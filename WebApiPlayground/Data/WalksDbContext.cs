using Microsoft.EntityFrameworkCore;
using WebApiPlayground.Models.Domain;

namespace WebApiPlayground.Data
{
    public class WalksDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public WalksDbContext(DbContextOptions<WalksDbContext> dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            _configuration = configuration;
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var connectionString = _configuration.GetConnectionString("WalksConnectionString");

            /*By providing the assembly that contains your migrations, Entity Framework Core can locate and apply any pending migrations 
            to the database during application startup.This ensures that the database schema is in sync with your code's model.*/
            optionsBuilder.UseSqlServer(connectionString, options =>
            {
                options.MigrationsAssembly(typeof(WalksDbContext).Assembly.FullName);
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var difficulties = SeedDifficulties();
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            var regions = SeedRegions();
            modelBuilder.Entity<Region>().HasData(regions);
        }

        private List<Difficulty> SeedDifficulties()
        {
            return new List<Difficulty>
            {
                new Difficulty{
                    Id = Guid.Parse("c2fe857b-4580-4240-8430-8d5b2572846d"),
                    Name = "Easy"
                },
                new Difficulty{
                    Id = Guid.Parse("1a0d5a3b-f9aa-4330-a2b7-6bf02f9b74f2"),
                    Name = "Medium"
                },
                new Difficulty{
                    Id = Guid.Parse("a78906f9-fb0f-42bb-8498-0346d5d46cc5"),
                    Name = "Hard"
                },
            };
        }

        private List<Region> SeedRegions()
        {
            return new List<Region>
            {
                new Region
                {
                    Id = Guid.Parse("e489f49a-b7ae-4b5a-9e50-9d39e928f67a"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageUrl = "https://cdn.pixabay.com/photo/2014/03/05/10/21/cornwall-park-279966_1280.jpg"
                },
                new Region
                {
                    Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                    Name = "Northland",
                    Code = "NTL",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                    Name = "Southland",
                    Code = "STL",
                    RegionImageUrl = null
                },
            };
        }
    }
}
