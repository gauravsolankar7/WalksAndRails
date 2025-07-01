using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await regionRepository.GetAllRegionsAsync();

            //var regionsDto = new List<RegionDto>();
            //foreach (var region in regions)
            //{
            //    regionsDto.Add(new RegionDto()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        RegionImageUrl = region.RegionImageUrl
            //    });
            //}
            //var regionsDto = ;

            return Ok(mapper.Map<List<RegionDto>>(regions));
        }

        [HttpGet]
        [Route("{id:guid}")]
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
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var regionModel = mapper.Map<Region>(addRegionRequestDto);
            //    new Region
            //{
            //    Code = addRegionRequestDto.Code,
            //    Name = addRegionRequestDto.Name,
            //    RegionImageUrl = addRegionRequestDto.RegionImageUrl
            //};

            regionModel = await regionRepository.CreateRegionAsync(regionModel);

            var regionDto = mapper.Map<RegionDto>(regionModel);

            //    new RegionDto()
            //{
            //    Id = regionModel.Id,
            //    Code = regionModel.Code,
            //    Name = regionModel.Name,
            //    RegionImageUrl = regionModel.RegionImageUrl
            //};

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDto.Id }, regionDto); 
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionModel = mapper.Map<Region>(updateRegionRequestDto);
            //    new Region
            //{
            //    Code = updateRegionRequestDto.Code,
            //    Name = updateRegionRequestDto.Name,
            //    RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            //};

            regionModel = await regionRepository.UpdateRegionAsync(id, regionModel);

            if (regionModel == null)
            {
                return NotFound();
            }

            //var regionDto = ;
            //new RegionDto
            //{
            //    Id = regionModel.Id,
            //    Code = regionModel.Code,
            //    Name = regionModel.Name,
            //    RegionImageUrl = regionModel.RegionImageUrl
            //};

            return Ok(mapper.Map<RegionDto>(regionModel));
        }

        [HttpDelete]
        [Route("{id:guid}")]
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
