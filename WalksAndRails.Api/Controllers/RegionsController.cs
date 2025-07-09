using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WalksAndRails.Api.CustomActionFilters;
using WalksAndRails.Api.Models.Domain;
using WalksAndRails.Api.Models.DTOs;
using WalksAndRails.Api.Repositories;

namespace WalksAndRails.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository  regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(IRegionRepository regionRepository, 
            IMapper mapper,
            ILogger<RegionsController> logger)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET ALL Regions
        // GET: https://localhost:1234/api/Regions 

        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAllRegions()
        {
            logger.LogInformation("GetAllRegions action method called");

            logger.LogWarning("This is a warning log");

            // Get data from database - Domain model
            var regions = await regionRepository.GetAllRegionsAsync();

            // Return Dto
            logger.LogInformation($"Finished get all regions with data: {JsonSerializer.Serialize(regions)}");

            return Ok(mapper.Map<List<RegionDto>>(regions));
        }

        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            var region = await regionRepository.GetRegionByIdAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            //var regionDto = new RegionDto()
            //{
            //    Id = region.Id,
            //    Code = region.Code,
            //    Name = region.Name,
            //    RegionImageUrl = region.RegionImageUrl
            //};
            return Ok(mapper.Map<RegionDto>(region));
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map the DTO to the domain model
            var regionModel = mapper.Map<Region>(addRegionRequestDto);

            //Use domain model to create a new region
            regionModel = await regionRepository.CreateRegionAsync(regionModel);

            //Map the domain model back to DTO
            var regionDto = mapper.Map<RegionDto>(regionModel);

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Map the DTO to the domain model
            var regionModel = mapper.Map<Region>(updateRegionRequestDto);

            //check if region with given id exists
            regionModel = await regionRepository.UpdateRegionAsync(id, regionModel);

            //
            if (regionModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDto>(regionModel));
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var regionModel = await regionRepository.DeleteRegionAsync(id);

            if (regionModel == null)
            {
                return NotFound();
            }

            //var regionDto = ;
            //    new RegionDto
            //{
            //    Id = regionModel.Id,
            //    Code = regionModel.Code,
            //    Name = regionModel.Name,
            //    RegionImageUrl = regionModel.RegionImageUrl
            //};

            return Ok(mapper.Map<RegionDto>(regionModel));
        }
    }
}
