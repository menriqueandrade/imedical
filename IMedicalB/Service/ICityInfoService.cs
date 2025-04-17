using IMedicalB.Dto;
using IMedicalB.Model;

namespace IMedicalB.Service
{
    public interface ICityInfoService
    {
        Task InsertCityInfoAsync(CityInfo cityInfo);

        Task<IEnumerable<CityHistoryDto>> ConsultHistoryInfoAsync();
    }
}
