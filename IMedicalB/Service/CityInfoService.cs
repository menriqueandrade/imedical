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
        private readonly ILogger<CityInfoService> _logger;

        public CityInfoService(IConfiguration configuration, IOptions<SqlQueries> sqlOptions, ILogger<CityInfoService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _sqlQueries = sqlOptions.Value;
            _logger = logger;
        }



        public async Task InsertCityInfoAsync(CityInfo cityInfo)
        {
            _logger.LogInformation("Iniciando la inserción de información para la ciudad: {City}", cityInfo.City);

            try
            {
                using var connection = new SqlConnection(_connectionString);

                string query = _sqlQueries.InsertCityInfo;

                var parameters = new
                {
                    CityData = cityInfo.City ?? "",
                    Info = cityInfo.Current_Weather?.Condition ?? ""
                };

                await connection.ExecuteAsync(query, parameters);

                _logger.LogInformation("La ciudad {City} ha sido insertada correctamente en la base de datos", cityInfo.City);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al insertar la ciudad {City} en la base de datos", cityInfo.City);
                throw;
            }
        }

        public async Task<IEnumerable<CityHistoryDto>> ConsultHistoryInfoAsync()
        {
            _logger.LogInformation("Consultando historial de ciudades");

            try
            {
                using var connection = new SqlConnection(_connectionString);

                string query = _sqlQueries.SelectCityHistory;

                var result = await connection.QueryAsync<CityHistoryDto>(query);

                _logger.LogInformation("Se han recuperado {Count} registros del historial de ciudades", result.Count());

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el historial de ciudades");
                throw;
            }
        }
    }
}
