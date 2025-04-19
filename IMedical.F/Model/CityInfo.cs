using System.Collections.Generic;

namespace IMedicalB.Model
{
    public class CityInfo
    {
        public string City { get; set; }
        public CurrentWeather Current_Weather { get; set; }
        public List<NewsItem> News { get; set; }
    }
}
