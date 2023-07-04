using Microsoft.AspNetCore.Mvc;
using WebApiPlayground.Data;
using WebApiPlayground.Models.Domain;
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

        [HttpGet("GetRegionById/{id:Guid}")]
        public IActionResult GetRegionById(Guid id)
        {
            var regionDomain = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (regionDomain == null)
                return NotFound();

            //Map Domain Models to DTOs
            var regionDto = new RegionDto
            {
                Id = id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };
            return Ok(regionDto); // We can send empty Ok() so there is no need to create Dto
        }

        [HttpPost("CreateNewRegion")]
        public IActionResult PostRegion(AddRegionRequestDto regionAddDto)
        {
            // Map DTO to Domain Model
            var newRegion = new Region
            {
                Code = regionAddDto.Code,
                Name = regionAddDto.Name,
                RegionImageUrl = regionAddDto.RegionImageUrl
            };

            // Use Domain Model to create a Region
            try
            {
                _dbContext.Regions.Add(newRegion);
                _dbContext.SaveChanges();

                // Map Domain model back to DTO
                var regionDto = new RegionDto
                {
                    Id = newRegion.Id,
                    Name = newRegion.Name,
                    Code = newRegion.Code,
                    RegionImageUrl = newRegion.RegionImageUrl
                };
                return CreatedAtAction(nameof(GetRegionById), new { id = newRegion.Id }, regionDto); // Returns 201 and Add 'Location' to 'Response Header'.
                //return Ok(regionDto ); returns 200 and the created Object
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateRegion/{id:Guid}")]
        public IActionResult UpdateRegion(Guid id, [FromBody] UpdateRegionDto regionUpdateDto)
        {
            var regionDomain = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (regionDomain == null)
                return NotFound();

            // Map DTO to Domain Model
            regionDomain.Name = regionUpdateDto.Name;
            regionDomain.Code = regionUpdateDto.Code;
            regionDomain.RegionImageUrl = regionUpdateDto.RegionImageUrl;

            try
            {
                _dbContext.SaveChanges();

                //Convert Domain Model to DTO
                var regionDto = new RegionDto
                {
                    Id = regionDomain.Id,
                    Name = regionDomain.Name,
                    Code = regionDomain.Code,
                    RegionImageUrl = regionUpdateDto.RegionImageUrl
                };
                return Ok(regionDto); 
            }
            catch
            {
                return BadRequest();
            } 
        }

        [HttpDelete("DeleteRegionById/{id:Guid}")]
        public IActionResult DeleteRegionById(Guid id)
        {
            var regionDomain = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (regionDomain == null)
                return NotFound();

            try
            {
                _dbContext.Regions.Remove(regionDomain);
                _dbContext.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}