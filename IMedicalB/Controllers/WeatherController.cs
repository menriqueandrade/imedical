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

        private readonly ICityInfoService _cityinfoService;

        public WeatherController(IWeatherService weatherService, ICityInfoService cityinfoService)
        {
            _weatherService = weatherService;
            _cityinfoService = cityinfoService;
        }

        [HttpGet("cities")]
        public async Task<IActionResult> GetAllWeather()
        {
            var result = await _weatherService.GetWeatherDataAsync();
            return Ok(result);
        }

        [HttpGet("city/{name}")]
        public async Task<IActionResult> GetWeatherByCityAsync(string name)
        {
            var result = await _weatherService.GetWeatherByCityAsync(name);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertCityWeather([FromBody] CityInfo cityInfo)
        {
            await _cityinfoService.InsertCityInfoAsync(cityInfo);
            return Ok("Historial insertado correctamente.");
        }

        [HttpPost("consult")]
        public async Task<IActionResult> ConsultHistoryInfoAsync()
        {
            var history = await _cityinfoService.ConsultHistoryInfoAsync();

            return Ok(new { history });
        }
    }
}
