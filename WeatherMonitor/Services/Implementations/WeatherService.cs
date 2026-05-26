using System;
using System.Linq;
using WeatherMonitor.Models;
using WeatherMonitor.Repositories.Interfaces;
using WeatherMonitor.Services.Interfaces;

namespace WeatherMonitor.Services.Implementations
{
    public class WeatherService : IWeatherService
    {
        private readonly IBaseRepository<WeatherReading> _repository;
        private readonly Random _random;

        private const double InitialTemperature = 15.0;
        private const double InitialHumidity = 60.0;
        private const double InitialPressure = 1013.25;

        public WeatherService(IBaseRepository<WeatherReading> repository)
        {
            _repository = repository;
            _random = new Random();
        }

        public WeatherReading GenerateReading()
        {
            var allReadings = _repository.GetAll();
            var lastReading = allReadings.OrderByDescending(r => r.Timestamp).FirstOrDefault();

            double currentTemp, currentHumidity, currentPressure;

            if (lastReading != null)
            {
                double tempChange = (_random.NextDouble() * 0.04) - 0.02;
                currentTemp = lastReading.Temperature + tempChange;

                double humChange = (_random.NextDouble() * 0.1) - 0.05; 
                currentHumidity = lastReading.Humidity + humChange;

                double pressChange = (_random.NextDouble() * 0.02) - 0.01;
                currentPressure = lastReading.Pressure + pressChange;

                currentTemp = Math.Clamp(currentTemp, -15.0, 35.0);
                currentHumidity = Math.Clamp(currentHumidity, 10.0, 100.0);
                currentPressure = Math.Clamp(currentPressure, 970.0, 1040.0);
            }
            else
            {
                currentTemp = InitialTemperature;
                currentHumidity = InitialHumidity;
                currentPressure = InitialPressure;
            }

            var newReading = new WeatherReading
            {
                Id = Guid.NewGuid(),
                Temperature = currentTemp,
                Humidity = currentHumidity,
                Pressure = currentPressure,
                Timestamp = DateTime.UtcNow
            };

            return _repository.Create(newReading);
        }
    }
}