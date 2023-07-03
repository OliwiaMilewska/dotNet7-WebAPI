using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using WebApiPlayground.Models.Domain;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

            var connectionString = _configuration.GetConnectionString("WalksConnectionString");

            /*By providing the assembly that contains your migrations, Entity Framework Core can locate and apply any pending migrations 
            to the database during application startup.This ensures that the database schema is in sync with your code's model.*/
            optionsBuilder.UseSqlServer(connectionString, options =>
            {
                options.MigrationsAssembly(typeof(WalksDbContext).Assembly.FullName);
            });
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
    }
}
