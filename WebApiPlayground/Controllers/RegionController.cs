using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiPlayground.Data;
using WebApiPlayground.Models.Domain;
using WebApiPlayground.Models.DTOs;
using WebApiPlayground.Repositories;

namespace WebApiPlayground.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly WalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;

        public RegionController(WalksDbContext dbContext, IRegionRepository regionRepository)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
        }

        [HttpGet("GetAllRegions")]
        public async Task<IActionResult> GetAllRegions()
        {
            // Get Data from Database
            var regionsDomain = await _regionRepository.GetAllRegionsAsync();

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
            var regionDomain = await _regionRepository.GetRegionByIdAsync(id);
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
                newRegion = await _regionRepository.CreateRegionAsync(newRegion);

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
            // Map Dto to Domain model
            var regionDomain = new Region
            {
                Name = regionUpdateDto.Name,
                Code = regionUpdateDto.Code,
                RegionImageUrl = regionUpdateDto.RegionImageUrl
            };

            var updatedRegion = await _regionRepository.UpdateRegionAsync(id, regionDomain);
            if (updatedRegion == null)
                return NotFound();

            // Map Domain to DTO
            var regionDto = new RegionDto
            {
                Id = updatedRegion.Id,
                Name = updatedRegion.Name,
                Code = updatedRegion.Code,
                RegionImageUrl = updatedRegion.RegionImageUrl
            };
            return Ok(regionDto);
        }

        [HttpDelete("DeleteRegionById/{id:Guid}")]
        public async Task<IActionResult> DeleteRegionById(Guid id)
        {
            var regionDomain = await _regionRepository.DeleteRegionByIdAsync(id);
            if (regionDomain == null)
                return NotFound();

            // Map Domain to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };
            return Ok(regionDto);
        }
    }
}