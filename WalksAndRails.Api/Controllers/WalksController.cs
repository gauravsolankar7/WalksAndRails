using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            var walkModel = mapper.Map<Walk>(addWalkRequestDto);
            
            await walkRepository.CreateWalkAsync(walkModel);

            return Ok(mapper.Map<WalkDto>(walkModel));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalks()
        {
            var walksModel = await walkRepository.GetAllWalksAsync();

            return Ok(mapper.Map<List<WalkDto>>(walksModel));
        }

        [HttpGet]
        [Route("{id:guid}")]
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
        public async Task<IActionResult> UpdateWalK([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            var walkModel = mapper.Map<Walk>(updateWalkRequestDto);

            walkModel = await walkRepository.UpdateWalkAsync(id, walkModel);

            if (walkModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDto>(walkModel));
        }

        [HttpDelete]
        [Route("{id:guid}")]
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
