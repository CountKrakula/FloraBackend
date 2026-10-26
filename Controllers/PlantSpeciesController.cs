using FloraBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FloraBackend.Controllers
{

    [ApiController]
    [Route("api/plant-species")]
    public class PlantSpeciesController : ControllerBase
    {
        private readonly PlantSpeciesService _plantSpeciesService;

        public PlantSpeciesController(PlantSpeciesService plantSpeciesService)
        {
            _plantSpeciesService = plantSpeciesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlantsAsync([FromQuery] string? query = null, [FromQuery][Range(1, int.MaxValue)] int page = 1)
        {
            var result = await _plantSpeciesService.GetPlantsAsync(query, page);

            return Content(result, "application/json");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlantByIdAsync([FromRoute] int id)
        {
            var result = await _plantSpeciesService.GetPlantByIdAsync(id);
            return Content(result, "application/json");
        }

    }
}