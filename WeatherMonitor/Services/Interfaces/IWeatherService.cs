using WeatherMonitor.Models;

namespace WeatherMonitor.Services.Interfaces
{
    public interface IWeatherService
    {
        WeatherReading GenerateReading();
    }
}