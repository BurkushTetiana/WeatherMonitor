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

        // Стартові значення на випадок, якщо база даних повністю порожня
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
            // 1. Намагаємося отримати найостанніший запис із бази даних
            var allReadings = _repository.GetAll();
            var lastReading = allReadings.OrderByDescending(r => r.Timestamp).FirstOrDefault();

            double currentTemp, currentHumidity, currentPressure;

            if (lastReading != null)
            {
                // 2. Якщо дані є, коливаємося навколо них в межах ±0.5 для температури
                double tempChange = (_random.NextDouble() * 1.0) - 0.5; // значення від -0.5 до +0.5
                currentTemp = lastReading.Temperature + tempChange;

                // Для краси також згладимо вологість (±1%) та тиск (±0.2 hPa)
                double humChange = (_random.NextDouble() * 2.0) - 1.0;
                currentHumidity = lastReading.Humidity + humChange;

                double pressChange = (_random.NextDouble() * 0.4) - 0.2;
                currentPressure = lastReading.Pressure + pressChange;

                // Обмежуємо значення розумними природними рамками, щоб вони не йшли в нескінченність
                currentTemp = Math.Clamp(currentTemp, -15.0, 35.0);
                currentHumidity = Math.Clamp(currentHumidity, 10.0, 100.0);
                currentPressure = Math.Clamp(currentPressure, 970.0, 1040.0);
            }
            else
            {
                // 3. Якщо база порожня — задаємо початкові стабільні показники
                currentTemp = InitialTemperature;
                currentHumidity = InitialHumidity;
                currentPressure = InitialPressure;
            }

            // Створюємо новий об'єкт для запису
            var newReading = new WeatherReading
            {
                Id = Guid.NewGuid(),
                Temperature = currentTemp,
                Humidity = currentHumidity,
                Pressure = currentPressure,
                Timestamp = DateTime.UtcNow
            };

            // Зберігаємо його в базу даних через репозиторій
            return _repository.Create(newReading);
        }
    }
}