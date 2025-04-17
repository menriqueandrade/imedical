using IMedicalB.Model;
using System.Text.Json;
using Dapper;
using System.Data.SqlClient;
using IMedicalB.Dto;


namespace IMedicalB.Service
{
    public class CityInfoService : ICityInfoService
    {
        private readonly string _connectionString;

        public CityInfoService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }



        public async Task InsertCityInfoAsync(CityInfo cityInfo)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = "INSERT INTO IMedical.dbo.weatherhistory (city, info, date_register) " +
                           "VALUES (@CityData, @Info, GETDATE())";

            var parameters = new
            {
                CityData = cityInfo.City ?? "",
                Info = cityInfo.Current_Weather?.Condition ?? ""
            };

            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<IEnumerable<CityHistoryDto>> ConsultHistoryInfoAsync()
        {
            using var connection = new SqlConnection(_connectionString);

            string query = "SELECT city, info FROM IMedical.dbo.weatherhistory";

            var result = await connection.QueryAsync<CityHistoryDto>(query);

            return result;
        }
    }
}
