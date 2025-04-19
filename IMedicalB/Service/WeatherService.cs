using System.Text.Json;
using IMedicalB.Model;
using Microsoft.Extensions.Options;

namespace IMedicalB.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        private readonly ICityInfoService _cityInfoService;

        private readonly ApiEndpoints _endpoints;

        public WeatherService(HttpClient httpClient, ICityInfoService cityInfoService, IOptions<ApiEndpoints> options)
        {
            _httpClient = httpClient;
            _cityInfoService = cityInfoService;
            _endpoints = options.Value;
        }

        public async Task<List<CityInfo>?> GetWeatherDataAsync()
        {                                             
            var response = await _httpClient.GetAsync(_endpoints.CityWeather);
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

            var cityData = allData?.FirstOrDefault(c => c.City.Equals(cityName, StringComparison.OrdinalIgnoreCase));

            if (cityData == null)
            {
                return null;  
            }

            await _cityInfoService.InsertCityInfoAsync(cityData);

            return cityData;
        }

        public async Task<CityInfo?> GetWeatherByCitySearhBarAsync(string cityName)
        {
            var allData = await GetWeatherDataAsync();

            var cityData = allData?.FirstOrDefault(c => c.City.Equals(cityName, StringComparison.OrdinalIgnoreCase));

            if (cityData == null)
            {
                return null;
            }

            return cityData;
        }
    }
}
