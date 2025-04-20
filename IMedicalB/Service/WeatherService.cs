using System.Text.Json;
using IMedicalB.Model;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;  // Incluir el espacio de nombres de logging

namespace IMedicalB.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly ICityInfoService _cityInfoService;
        private readonly ApiEndpoints _endpoints;
        private readonly ILogger<WeatherService> _logger;  // Inyección de ILogger

        public WeatherService(HttpClient httpClient, ICityInfoService cityInfoService, IOptions<ApiEndpoints> options, ILogger<WeatherService> logger)
        {
            _httpClient = httpClient;
            _cityInfoService = cityInfoService;
            _endpoints = options.Value;
            _logger = logger;  // Asignación del logger
        }

        public async Task<List<CityInfo>?> GetWeatherDataAsync()
        {
            _logger.LogInformation("Iniciando la obtención de datos climáticos desde la API: {ApiUrl}", _endpoints.CityWeather);

            try
            {
                var response = await _httpClient.GetAsync(_endpoints.CityWeather);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<CityInfo>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _logger.LogInformation("Datos obtenidos correctamente desde la API para el clima");

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los datos climáticos desde la API");
                throw;
            }
        }

        public async Task<CityInfo?> GetWeatherByCityAsync(string cityName)
        {
            _logger.LogInformation("Buscando datos climáticos para la ciudad: {CityName}", cityName);

            var allData = await GetWeatherDataAsync();

            var cityData = allData?.FirstOrDefault(c => c.City.Equals(cityName, StringComparison.OrdinalIgnoreCase));

            if (cityData == null)
            {
                _logger.LogWarning("No se encontraron datos para la ciudad: {CityName}", cityName);
                return null;
            }

            // Insertar datos en la base de datos
            await _cityInfoService.InsertCityInfoAsync(cityData);

            _logger.LogInformation("Datos climáticos para la ciudad {CityName} insertados correctamente en la base de datos", cityName);

            return cityData;
        }

        public async Task<CityInfo?> GetWeatherByCitySearhBarAsync(string cityName)
        {
            _logger.LogInformation("Buscando datos climáticos para la ciudad (desde barra de búsqueda): {CityName}", cityName);

            var allData = await GetWeatherDataAsync();

            var cityData = allData?.FirstOrDefault(c => c.City.Equals(cityName, StringComparison.OrdinalIgnoreCase));

            if (cityData == null)
            {
                _logger.LogWarning("No se encontraron datos para la ciudad (desde barra de búsqueda): {CityName}", cityName);
                return null;
            }

            _logger.LogInformation("Datos climáticos para la ciudad {CityName} encontrados correctamente", cityName);

            return cityData;
        }
    }
}
