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

        [HttpGet]
        public IActionResult GetWeather()
        {
            var weather = _weatherService.GetWeather();
            return Ok(weather);
        }
    }
}
