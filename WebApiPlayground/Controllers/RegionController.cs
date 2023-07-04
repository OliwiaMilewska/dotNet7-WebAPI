using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IMapper _mapper;

        public RegionController(WalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAllRegions")]
        public async Task<IActionResult> GetAllRegions()
        {
            // Get Data from Database
            var regionsDomain = await _regionRepository.GetAllRegionsAsync();
            if (regionsDomain.Count() == 0)
                return NotFound();

            //Map Domain Models to DTOs
            /*var regionsDto = new List<RegionDto>();
            foreach (var region in regionsDomain)
            {
                regionsDto.Add(new RegionDto
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl
                });
            }*/
            var regionsDto = _mapper.Map<List<RegionDto>>(regionsDomain);

            // Return DTOs
            return Ok(regionsDto);
        }

        [HttpGet("GetRegionById/{id:Guid}")]
        public async Task<IActionResult> GetRegionById(Guid id)
        {
            var regionDomain = await _regionRepository.GetRegionByIdAsync(id);
            if (regionDomain == null)
                return NotFound();

            var regionDto = _mapper.Map<RegionDto>(regionDomain);

            return Ok(regionDto); // We can send empty Ok() so there is no need to create Dto.
        }

        [HttpPost("CreateNewRegion")]
        public async Task<IActionResult> CreateRegion(AddRegionRequestDto regionAddDto)
        {
            var newRegion = _mapper.Map<Region>(regionAddDto);
            try
            {
                newRegion = await _regionRepository.CreateRegionAsync(newRegion);
                var regionDto = _mapper.Map<Region>(newRegion);
                return CreatedAtAction(nameof(GetRegionById), new { id = newRegion.Id }, regionDto); // Returns 201 and Adds 'Location' to 'Response Header'.
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
            var regionDomain = _mapper.Map<Region>(regionUpdateDto);

            var updatedRegion = await _regionRepository.UpdateRegionAsync(id, regionDomain);
            if (updatedRegion == null)
                return NotFound();

            var regionDto = _mapper.Map<RegionDto>(updatedRegion);

            return Ok(regionDto);
        }

        [HttpDelete("DeleteRegionById/{id:Guid}")]
        public async Task<IActionResult> DeleteRegionById(Guid id)
        {
            var regionDomain = await _regionRepository.DeleteRegionByIdAsync(id);
            if (regionDomain == null)
                return NotFound();

            var regionDto = _mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }
    }
}