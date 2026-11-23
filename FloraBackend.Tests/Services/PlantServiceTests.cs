using FloraBackend.DTOs.Plants;
using FloraBackend.Models;
using FloraBackend.Repositories.IRepository;
using FloraBackend.Services;
using Moq;

namespace FloraBackend.Tests.Services;

public class PlantServiceTests
{
    [Fact]
    public async Task CreateAsync_SetsUserIdAndCreatedAt_ForLoggedInUser()
    {
        // Arrange
        var plantRepoMock = new Mock<IPlantRepository>();

        // The repository returns the same plant it gets, like the real one does
        plantRepoMock.Setup(repo => repo.AddAsync(It.IsAny<Plant>()))
            .ReturnsAsync((Plant plant) => plant);

        var plantService = new PlantService(plantRepoMock.Object);
        var dto = new CreatePlantDto { Name = "Monstera", Location = "Kitchen" };

        // Act
        var result = await plantService.CreateAsync(dto, 5);

        // Assert
        // The plant is saved with the logged in user's id
        plantRepoMock.Verify(
            repo => repo.AddAsync(It.Is<Plant>(p => p.UserId == 5)),
            Times.Once);

        // The returned plant has the values from the dto
        Assert.Equal("Monstera", result.Name);
        Assert.Equal("Kitchen", result.Location);

        // CreatedAt is set by the server, to now
        Assert.True((DateTime.UtcNow - result.CreatedAt).TotalSeconds < 5);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenPlantBelongsToAnotherUser()
    {
        // Arrange
        var plantRepoMock = new Mock<IPlantRepository>();

        // No plant with id 1 is found for user 2, because it belongs to someone else
        plantRepoMock.Setup(repo => repo.GetByIdAsync(1, 2))
            .ReturnsAsync((Plant?)null);

        var plantService = new PlantService(plantRepoMock.Object);
        var dto = new UpdatePlantDto { Name = "New name", Location = "Somewhere" };

        // Act
        var result = await plantService.UpdateAsync(1, dto, 2);

        // Assert
        Assert.False(result);

        // Nothing is saved to the database
        plantRepoMock.Verify(
            repo => repo.UpdateAsync(It.IsAny<Plant>()),
            Times.Never);
    }
}