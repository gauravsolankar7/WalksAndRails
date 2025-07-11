using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalksAndRails.Api.CustomActionFilters;
using WalksAndRails.Api.Models.Domain;
using WalksAndRails.Api.Models.DTOs;
using WalksAndRails.Api.Repositories;

namespace WalksAndRails.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //Map the DTO to the domain model
            var walkModel = mapper.Map<Walk>(addWalkRequestDto);

            await walkRepository.CreateWalkAsync(walkModel);

            // Map the domain model back to the DTO
            return Ok(mapper.Map<WalkDto>(walkModel));
        }

        //GET Walks
        //GET : api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&
        //pageNumber=1&pageSize=10
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAllWalks(
            [FromQuery] string? filterOn, string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending, 
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksModel = await walkRepository.GetAllWalksAsync(filterOn, filterQuery, 
                sortBy, isAscending ?? true, pageNumber, pageSize);

            //Map the domain model to DTO
            return Ok(mapper.Map<List<WalkDto>>(walksModel));
        }

        [HttpGet]
        [Route("{id:guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
        {
            var walkModel = await walkRepository.GetWalkByIdAsync(id);

            if (walkModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDto>(walkModel));
        }

        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateWalK([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            // Map the DTO to the domain model
            var walkModel = mapper.Map<Walk>(updateWalkRequestDto);
            
            walkModel = await walkRepository.UpdateWalkAsync(id, walkModel);
            
            if (walkModel == null)
            {
                return NotFound();
            }
            // Map the updated domain model back to the DTO
            return Ok(mapper.Map<WalkDto>(walkModel));
        }

        [HttpDelete]
        [Route("{id:guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteWalk([FromRoute] Guid id)
        {
            var walkModel = await walkRepository.DeleteWalkAsync(id);

            if (walkModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDto>(walkModel));
        }
    }
}
