using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiPlayground.Data;
using WebApiPlayground.Models.Domain;

namespace WebApiPlayground.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly WalksDbContext _dbContext;
        public RegionController(WalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("GetAllRegions")]
        public IActionResult GetAllRegions()
        {
            var regions = _dbContext.Regions.ToList();
            return Ok(regions);
        }

        [HttpGet("GetRegionById/{id}")]
        public IActionResult GetRegionById(Guid id)
        {
            var region = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (region == null)
                return NotFound();
            return Ok(region);
        }
    }
}