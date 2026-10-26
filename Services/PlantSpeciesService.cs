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

        public async Task<string> GetPlantsAsync(string? query = null, int page = 1)
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
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                // Hantera fel här, t.ex. logga eller kasta ett undantag
                throw new Exception($"Error fetching data from Perenual API: {response.ReasonPhrase}");
            }
        }

        public async Task<string> SearchPlantAsync(string query)
        {
            // sök efter växt
            return string.Empty;
        }
    }
}
