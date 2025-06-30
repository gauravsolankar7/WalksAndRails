using Microsoft.AspNetCore.Mvc;
using WalksAndRails.Api.Data;
using WalksAndRails.Api.Models.Domain;
using WalksAndRails.Api.Models.DTOs;

namespace WalksAndRails.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public RegionsController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllRegions()
        {
            var regions = dbContext.Regions.ToList();

            var regionsDto = new List<RegionDto>();
            foreach (var region in regions)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                });
            }
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetRegionById([FromRoute] Guid id)
        {
            var region = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (region == null)
            {
                return NotFound();
            }

            var regionDto = new RegionDto()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };
            return Ok(regionDto);
        }

        [HttpPost]
        public IActionResult CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var regionModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            dbContext.Regions.Add(regionModel);
            dbContext.SaveChanges();

            var regionDto = new RegionDto()
            {
                Id = regionModel.Id,
                Code = regionModel.Code,
                Name = regionModel.Name,
                RegionImageUrl = regionModel.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetRegionById), new { id = regionModel.Id }, regionModel);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionModel == null)
            {
                return NotFound();
            }

            regionModel.Code = updateRegionRequestDto.Code;
            regionModel.Name = updateRegionRequestDto.Name;
            regionModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            dbContext.SaveChanges();

            var regionDto = new RegionDto
            {
                Id = regionModel.Id,
                Code = regionModel.Code,
                Name = regionModel.Name,
                RegionImageUrl = regionModel.RegionImageUrl
            };
            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult DeleteRegion([FromRoute] Guid id)
        {
            var regionModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (regionModel == null)
            {
                return NotFound();
            }
            dbContext.Regions.Remove(regionModel);
            dbContext.SaveChanges();

            var regionDto = new RegionDto
            {
                Id = regionModel.Id,
                Code = regionModel.Code,
                Name = regionModel.Name,
                RegionImageUrl = regionModel.RegionImageUrl
            };
            return Ok(regionDto);
        }
    }
}
