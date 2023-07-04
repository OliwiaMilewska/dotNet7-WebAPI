using Microsoft.AspNetCore.Mvc;
using WebApiPlayground.Data;
using WebApiPlayground.Models.DTOs;

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
            // Get Data from Database
            var regionsDomain = _dbContext.Regions.ToList();

            //Map Domain Models to DTOs
            var regionsDto = new List<RegionDto>();
            foreach (var region in regionsDomain)
            {
                regionsDto.Add(new RegionDto
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl
                });
            }

            // Return DTOs
            return Ok(regionsDto);
        }

        [HttpGet("GetRegionById/{id}")]
        public IActionResult GetRegionById(Guid id)
        {
            var regionDomain = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (regionDomain == null)
                return NotFound();

            var regionDto = new RegionDto
            {
                Id = id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };
            return Ok(regionDto);
        }
    }
}