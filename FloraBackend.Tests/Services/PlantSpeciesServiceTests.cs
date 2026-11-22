using FloraBackend.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;

namespace FloraBackend.Tests.Services;

public class PlantSpeciesServiceTests
{
    [Fact]
    public async Task GetPlantByIdAsync_ThrowsException_WhenIdIsBelowOne()
    {
        // Arrange
        var httpClient = new HttpClient();
        var configuration = new ConfigurationBuilder().Build();

        var service = new PlantSpeciesService(
            httpClient,
            configuration
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => service.GetPlantByIdAsync(0)
        );
    }
    [Fact]
    public async Task GetPlantsAsync_IncludesQueryInUrl_WhenSearching()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            //Svaret som vi ska få från handler, returnerar 200 OK och tom lista med plantor
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"data\": []}")
            });
        //handlerMock.Object är det fejkade HttpMessageHandler-objektet som skickas till HttpClient
        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://perenual.com/api/")
        };
        //Mocka konfiguration direkt i testet
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
            { "Perenual:ApiKey", "test-key" }
            })
            .Build();

        var service = new PlantSpeciesService(httpClient, configuration);

        // Act
        await service.GetPlantsAsync("monstera");

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(
                request => request.RequestUri != null &&
                           request.RequestUri.Query.Contains("q=monstera")
            ),
            ItExpr.IsAny<CancellationToken>()
        );
    }
}

