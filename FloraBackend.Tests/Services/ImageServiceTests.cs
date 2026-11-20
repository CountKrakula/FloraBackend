using FloraBackend.DTOs.Images;
using FloraBackend.Models;
using FloraBackend.Repositories.IRepository;
using FloraBackend.Services;
using Microsoft.AspNetCore.Http;
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

        [Fact]
        public async Task UpdateImage_WhenImageDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var mockRepo = new Mock<IImageRepository>();
            var mockPlantRepo = new Mock<IPlantRepository>();

            int imageId = 1;
            int userId = 1;

            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(1024);
            mockFile.Setup(f => f.ContentType).Returns("image/jpeg");
            mockFile.Setup(f => f.FileName).Returns("test.jpg");

            // Image doesn't exist - return null
            mockRepo.Setup(repo => repo.GetImage(imageId))
                .ReturnsAsync((Image)null);

            var imageService = new ImageService(mockRepo.Object, mockPlantRepo.Object);

            // Act
            var result = await imageService.UpdateImage(imageId, mockFile.Object, userId);

            // Assert
            Assert.False(result.isSuccessful);
            Assert.Contains("Image not found", result.Errors);
        }
    }
}
