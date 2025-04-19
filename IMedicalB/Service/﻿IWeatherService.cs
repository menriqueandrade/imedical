using IMedicalB.Model;

namespace IMedicalB.Service
{
    public interface IWeatherService
    {
        Task<List<CityInfo>?> GetWeatherDataAsync();

        Task<CityInfo?> GetWeatherByCityAsync(string cityName);

        Task<CityInfo?> GetWeatherByCitySearhBarAsync(string cityName);
    }
}
