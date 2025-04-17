using System.Text.Json;
using IMedicalB.Model;

namespace IMedicalB.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CityInfo>?> GetWeatherDataAsync()
        {
            var response = await _httpClient.GetAsync("https://68012dd781c7e9fbcc41c722.mockapi.io/api/v1/weather/cityweather");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<CityInfo>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return data;
        }

        public async Task<CityInfo?> GetWeatherByCityAsync(string cityName)
        {
            var allData = await GetWeatherDataAsync();
            return allData.FirstOrDefault(c => c.City.Equals(cityName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
