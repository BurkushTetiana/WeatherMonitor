using Moq;
using Xunit;
using WeatherMonitor.Models;
using WeatherMonitor.Services.Interfaces;
using WeatherMonitor.Repositories.Interfaces;
using WeatherMonitor.Services.Implementations;

namespace WeatherMonitor_Tests
{
    public class WeatherServiceTests
    {
        [Fact]
        public void GenerateReading_CreatesAndReturnsReading()
        {
            var mockRepo = new Mock<IBaseRepository<WeatherReading>>();
            mockRepo.Setup(x => x.Create(It.IsAny<WeatherReading>()))
                    .Returns((WeatherReading r) => r);

            var service = new WeatherService(mockRepo.Object);
            var result = service.GenerateReading();

            Assert.NotNull(result);
            Assert.InRange(result.Temperature, -10, 30);
            Assert.InRange(result.Humidity, 0, 100);
            Assert.InRange(result.Pressure, 980, 1020);
        }
    }
}