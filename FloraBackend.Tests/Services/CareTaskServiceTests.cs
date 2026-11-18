using FloraBackend.Models;
using FloraBackend.Repositories.IRepository;
using FloraBackend.Services;
using FloraBackend.DTOs.CareTasks;
using Moq;

namespace FloraBackend.Tests.Services;

public class CareTaskServiceTests
{
    [Fact]
    public async Task CompleteAsync_SetsLastDoneAndNextDue_WhenTaskExists()
    {
        // Arrange
        // GetByIdAsync(plantId, userId)
        var mockPlant = new Plant { UserId = 1, PlantId = 1 };
        var mockCareTask = new CareTask { Id = 1, PlantId = 1, IntervalDays = 7 };

        var repoMock = new Mock<IPlantRepository>();
        var careTaskRepoMock = new Mock<ICareTaskRepository>();
        
        repoMock.Setup(repo => repo.GetByIdAsync(mockPlant.PlantId, mockPlant.UserId))
            .ReturnsAsync(mockPlant);
        
        careTaskRepoMock.Setup(repo => repo.GetByIdAsync(mockCareTask.Id, mockCareTask.PlantId))
            .ReturnsAsync(mockCareTask);

        var careTaskServiceMock = new CareTaskService(careTaskRepoMock.Object, repoMock.Object);

        // Act
        var result = await careTaskServiceMock.CompleteAsync(1, 1,1);


        // Assert
        // Checks that you get a completed care task 
        Assert.NotNull(result);
        Assert.IsType<CareTaskDto>(result);
        Assert.NotNull(result.LastDoneAt);
        // Check that next due at is correct according to interval days
        Assert.Equal(result.LastDoneAt.Value.AddDays(7), result.NextDueAt);


    }

    [Fact]
    public async Task CompleteAsync_ReturnsNull_WhenPlantNotFound()
    {
        //Arrange 
        var repoMock = new Mock<IPlantRepository>();
        var careTaskRepoMock = new Mock<ICareTaskRepository>();
        
        repoMock.Setup(repo => repo.GetByIdAsync(1, 1))
            .ReturnsAsync((Plant?)null);
        
        var careTaskServiceMock = new CareTaskService(careTaskRepoMock.Object, repoMock.Object);
        
        //Act 
        var result = await careTaskServiceMock.CompleteAsync(1, 1,1);

        
        // Assert
        Assert.Null(result);
    } 
    
}