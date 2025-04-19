using IMedicalB.Model;
using System.Text.Json;
using Dapper;
using System.Data.SqlClient;
using IMedicalB.Dto;
using IMedicalB.Sql;
using Microsoft.Extensions.Options;


namespace IMedicalB.Service
{
    public class CityInfoService : ICityInfoService
    {
        private readonly string _connectionString;
        private readonly SqlQueries _sqlQueries;

        public CityInfoService(IConfiguration configuration, IOptions<SqlQueries> sqlOptions)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _sqlQueries = sqlOptions.Value;
        }



        public async Task InsertCityInfoAsync(CityInfo cityInfo)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = _sqlQueries.InsertCityInfo;

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

            string query = _sqlQueries.SelectCityHistory;

            var result = await connection.QueryAsync<CityHistoryDto>(query);

            return result;
        }
    }
}
