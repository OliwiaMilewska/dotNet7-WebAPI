using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApiPlayground.CustomActionFilters;
using WebApiPlayground.Models.Domain;
using WebApiPlayground.Models.DTOs;
using WebApiPlayground.Repositories;

namespace WebApiPlayground.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
        }

        [HttpGet]
        // GET: /api/walks?filterOn=Name&filterQuery=Track
        public async Task<IActionResult> GetAllWalks([FromQuery] string? filterOn, [FromQuery] string? filterQuery)
        {
            var walksDomain = await _walkRepository.GetAllWalksAsync(filterOn, filterQuery);
            var walksDto = _mapper.Map<List<WalkDto>>(walksDomain);
            return Ok(walksDto);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetWalkById(Guid id)
        {
            var walkDomain = await _walkRepository.GetWalkByIdAsync(id);
            if (walkDomain == null)
                return NotFound();

            var walkDto = _mapper.Map<WalkDto>(walkDomain);

            return Ok(walkDto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateWalk([FromBody] ModifyWalkDto newWalk)
        {
            var walkDomainModel = _mapper.Map<Walk>(newWalk);
            walkDomainModel = await _walkRepository.CreateWalkAsync(walkDomainModel);
            var walkDto = _mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walkDto);
        }

        [HttpPut("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateWalk(Guid id, [FromBody] ModifyWalkDto walkModifyDto)
        {
            var walkDomain = _mapper.Map<Walk>(walkModifyDto);
            walkDomain = await _walkRepository.UpdateWalkAsync(id, walkDomain);
            if (walkDomain == null)
                return NotFound();

            var walkDto = _mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteWalk(Guid id)
        {
            var walkDomain = await _walkRepository.DeleteWalkByIdAsync(id);
            if (walkDomain == null)
                return NotFound();

            var walkDto = _mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
        }
    }
}