using FloraBackend.DTOs.PlantSpecies;

namespace FloraBackend.Services
{
    public class PlantSpeciesService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PlantSpeciesService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<PlantSpeciesListResponseDTO?> GetPlantsAsync(string? query = null, int page = 1)
        {
            var apiKey = _configuration["Perenual:ApiKey"]
                ?? throw new InvalidOperationException(
                    "Perenual API key is missing."
                );
            var url = $"species-list?key={apiKey}&page={page}";

            if (!string.IsNullOrWhiteSpace(query))
            {
                url += $"&q={Uri.EscapeDataString(query)}";
            }
            using var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<PlantSpeciesListResponseDTO>();
        }

        public async Task<PlantSpeciesDetailsDTO?> GetPlantByIdAsync(int id)
        {
            // free version of the Perenual API only allows for plant species IDs between 1 and 3000
            if (id < 1 || id > 3000)
            {
                throw new ArgumentOutOfRangeException(nameof(id),
                    "Unable to display details for this plant.");
            }
            var apiKey = _configuration["Perenual:ApiKey"]
                ?? throw new InvalidOperationException(
                    "Perenual API key is missing."
                );
            var url = $"species/details/{id}";
            var response = await _httpClient.GetAsync($"{url}?key={apiKey}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PlantSpeciesDetailsDTO>();
            }
            else
            {
                throw new Exception($"Error fetching data from Perenual API: {response.ReasonPhrase}");
            }
        }
    }
}
