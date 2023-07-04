using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> GetAllRegions()
        {
            // Get Data from Database
            var regionsDomain = await _dbContext.Regions.ToListAsync();

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
        public async Task<IActionResult> GetRegionById(Guid id)
        {
            var regionDomain = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
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
        public async Task<IActionResult> PostRegion(AddRegionRequestDto regionAddDto)
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
                await _dbContext.Regions.AddAsync(newRegion);
                await _dbContext.SaveChangesAsync();

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
        public async Task<IActionResult> UpdateRegion(Guid id, [FromBody] UpdateRegionDto regionUpdateDto)
        {
            var regionDomain = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionDomain == null)
                return NotFound();

            // Map DTO to Domain Model
            regionDomain.Name = regionUpdateDto.Name;
            regionDomain.Code = regionUpdateDto.Code;
            regionDomain.RegionImageUrl = regionUpdateDto.RegionImageUrl;

            try
            {
                await _dbContext.SaveChangesAsync();

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
        public async Task<IActionResult> DeleteRegionById(Guid id)
        {
            var regionDomain = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionDomain == null)
                return NotFound();

            try
            {
                _dbContext.Regions.Remove(regionDomain); // Remove doesn't have a async method.
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}