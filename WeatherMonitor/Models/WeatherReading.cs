using System;
using WeatherMonitor.Models.Base;

namespace WeatherMonitor.Models
{
    public class WeatherReading : BaseModel
    {
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
        public DateTime Timestamp { get; set; }
    }
}