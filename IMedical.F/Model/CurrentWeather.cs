namespace IMedicalB.Model
{
    public class CurrentWeather
    {
        public int Temp { get; set; }
        public int Humidity { get; set; }
        public string Condition { get; set; }
        public string Country { get; set; }
        public int Wind_Speed { get; set; }
        public string Wind_Direction { get; set; }
        public int Feels_Like { get; set; }
    }
}
