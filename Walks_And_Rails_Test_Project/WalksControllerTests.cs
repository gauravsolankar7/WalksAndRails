using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WalksAndRails.Api.Controllers;
using WalksAndRails.Api.Models.Domain;
using WalksAndRails.Api.Models.DTOs;
using WalksAndRails.Api.Repositories;

namespace WalksAndRails.Api.Tests
{
    [TestFixture]
    public class WalksControllerTests
    {
        private Mock<IMapper> _mockMapper;
        private Mock<IWalkRepository> _mockRepo;
        private WalksController _controller;

        [SetUp]
        public void Setup()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepo = new Mock<IWalkRepository>();
            _controller = new WalksController(_mockMapper.Object, _mockRepo.Object);
        }

        [Test]
        public async Task CreateWalk_ShouldReturnOkResult_WithWalkDto()
        {
            var addDto = new AddWalkRequestDto
            {
                Name = "Test Walk", LengthInKm = 5.0, Description = "Test description", 
                WalkImageUrl = "http://example.com/image.jpg", DifficultyId = Guid.NewGuid(), 
                RegionId = Guid.NewGuid()
            };

            var difficulty = new Difficulty
            { Id = addDto.DifficultyId, Name = "Easy" };

            var region = new Region
            { Id = addDto.RegionId, Name = "Test Region", Code = "ABC" };

            var walkModel = new Walk
            { 
                Id = Guid.NewGuid(), Name = addDto.Name, Description = addDto.Description, 
                LengthInKm = addDto.LengthInKm, WalkImageUrl = addDto.WalkImageUrl, DifficultyId = addDto.DifficultyId,
                RegionId = addDto.RegionId, Difficulty = difficulty, Region = region
            };

            var walkDto = new WalkDto
            {
                Id = walkModel.Id, Name = walkModel.Name, Description = walkModel.Description, LengthInKm = walkModel.LengthInKm,
                WalkImageUrl = walkModel.WalkImageUrl, 
                Difficulty = new DifficultyDto { Id = difficulty.Id, Name = difficulty.Name },
                Region = new RegionDto { Id = region.Id, Name = region.Name, Code = region.Code }
            };

            _mockMapper.Setup(m => m.Map<Walk>(addDto)).Returns(walkModel);
            _mockRepo.Setup(r => r.CreateWalkAsync(walkModel)).ReturnsAsync(walkModel);
            _mockMapper.Setup(m => m.Map<WalkDto>(walkModel)).Returns(walkDto);

            var result = await _controller.CreateWalk(addDto) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);

            var returnedDto = result.Value as WalkDto;
            Assert.IsNotNull(returnedDto);
            Assert.AreEqual("Test Walk", returnedDto.Name);
            Assert.AreEqual("Test description", returnedDto.Description);
            Assert.AreEqual("http://example.com/image.jpg", returnedDto.WalkImageUrl);
            Assert.AreEqual("Easy", returnedDto.Difficulty?.Name);
            Assert.AreEqual("Test Region", returnedDto.Region?.Name);
        }


        [Test]
        public async Task GetWalkById_ShouldReturnNotFound_WhenWalkDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetWalkByIdAsync(id)).ReturnsAsync((Walk)null);

            // Act
            var result = await _controller.GetWalkById(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetWalkById_ShouldReturnOk_WhenWalkExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var walkModel = new Walk
            {
                Id = id,
                Name = "Existing Walk",
                LengthInKm = 10.0,
                Description = "Existing description",
                Difficulty = new Difficulty { Id = Guid.NewGuid(), Name = "Moderate" },
                Region = new Region { Id = Guid.NewGuid(), Name = "Existing Region", Code = "ABC"}
            };
            var walkDto = new WalkDto { Id = id, Name = "Existing Walk", LengthInKm = 10.0, Description = "Test Description"};

            _mockRepo.Setup(r => r.GetWalkByIdAsync(id)).ReturnsAsync(walkModel);
            _mockMapper.Setup(m => m.Map<WalkDto>(walkModel)).Returns(walkDto);

            // Act
            var result = await _controller.GetWalkById(id) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var returnedDto = result.Value as WalkDto;
            Assert.AreEqual("Existing Walk", returnedDto.Name);
        }
    }
}
