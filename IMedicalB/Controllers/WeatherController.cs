using IMedicalB.Model;
using IMedicalB.Service;
using Microsoft.AspNetCore.Mvc;

namespace IMedicalB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("cities")]
        public async Task<IActionResult> GetAllWeather()
        {
            var result = await _weatherService.GetWeatherDataAsync();
            return Ok(result);
        }

        [HttpGet("city/{name}")]
        public async Task<IActionResult> GetWeatherByCity(string name)
        {
            var result = await _weatherService.GetWeatherByCityAsync(name);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
