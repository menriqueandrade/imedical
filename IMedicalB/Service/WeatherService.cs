using IMedicalB.Model;

namespace IMedicalB.Service
{
    public class WeatherService : IWeatherService
    {

        public Weather GetWeather()
        {
            
            return new Weather
            {
                Id = "1",
                City = "New York",
                Temp = 18,
                Humidity = 65,
                Condition = "Partly Cloudy",
                Country = "US",
                Info = "Feels like 16°C. Wind: 15 km/h NE. Expect scattered showers in the afternoon."
            };
        }
    }
}
