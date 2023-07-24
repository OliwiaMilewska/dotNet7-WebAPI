using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApiPlayground.CustomActionFilters;
using WebApiPlayground.Models.Domain;
using WebApiPlayground.Models.DTOs;
using WebApiPlayground.Repositories;

namespace WebApiPlayground.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;
        private readonly ILogger<WalksController> _logger;

        public WalksController(IMapper mapper, IWalkRepository walkRepository, ILogger<WalksController> logger)
        {
            _walkRepository = walkRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        // GET: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        public async Task<IActionResult> GetAllWalksV1([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomain = await _walkRepository.GetAllWalksAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            var walksDto = _mapper.Map<List<WalkDtoV1>>(walksDomain);

            return Ok(walksDto);
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        // GET: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        public async Task<IActionResult> GetAllWalksV2([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomain = await _walkRepository.GetAllWalksAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            var walksDto = _mapper.Map<List<WalkDtoV1>>(walksDomain);

            // Testing middleware
            throw new Exception("This is testing exception");

            return Ok(walksDto);
        }

        [HttpGet]
        [MapToApiVersion("3.0")]
        // GET: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        public async Task<IActionResult> GetAllWalksV3([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomain = await _walkRepository.GetAllWalksAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            var walksDto = _mapper.Map<List<WalkDtoV2>>(walksDomain);

            return Ok(walksDto);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetWalkById(Guid id)
        {
            var walkDomain = await _walkRepository.GetWalkByIdAsync(id);
            if (walkDomain == null)
                return NotFound();

            var walkDto = _mapper.Map<WalkDtoV1>(walkDomain);

            return Ok(walkDto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateWalk([FromBody] ModifyWalkDto newWalk)
        {
            var walkDomainModel = _mapper.Map<Walk>(newWalk);
            walkDomainModel = await _walkRepository.CreateWalkAsync(walkDomainModel);
            var walkDto = _mapper.Map<WalkDtoV1>(walkDomainModel);
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

            var walkDto = _mapper.Map<WalkDtoV1>(walkDomain);
            return Ok(walkDto);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteWalk(Guid id)
        {
            var walkDomain = await _walkRepository.DeleteWalkByIdAsync(id);
            if (walkDomain == null)
                return NotFound();

            var walkDto = _mapper.Map<WalkDtoV1>(walkDomain);
            return Ok(walkDto);
        }
    }
}