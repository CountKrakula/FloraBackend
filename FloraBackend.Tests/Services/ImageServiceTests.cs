using FloraBackend.DTOs.Images;
using FloraBackend.Models;
using FloraBackend.Repositories.IRepository;
using FloraBackend.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FloraBackend.Tests.Services
{
    public class ImageServiceTests
    {
        [Fact]
        public async Task GetImageByUserIdAsync_WhenImageExists()
        {
            //Arrange
            var mockPlant = new Plant { UserId = 1, PlantId = 1 };
            var mockImage = new Image { ImageId = 1, Url= "/uploads/b587403f-f4ed-4def-90ea-c278a7f268e5.jpg", PlantId = 1 };

            var mockRepo = new Mock<IPlantRepository>();
            var mockImageRepo = new Mock<IImageRepository>();

            mockRepo.Setup(repo => repo.GetByIdAsync(mockPlant.PlantId, mockPlant.UserId))
                .ReturnsAsync(mockPlant);

            mockImageRepo.Setup(repo => repo.GetImageByUserId(mockPlant.UserId, mockImage.PlantId))
                .ReturnsAsync(new List<Image> { mockImage });

            var mockImageService = new ImageService(mockImageRepo.Object, mockRepo.Object);

            //Act
            var result = await mockImageService.GetUserImages(1, 1);

            //Assert
            Assert.NotNull(result);
        }
    }
}
