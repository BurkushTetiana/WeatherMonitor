using Moq;
using System.Collections.Generic;
using WeatherMonitor.Models;
using WeatherMonitor.Repositories.Interfaces;
using WeatherMonitor.Services.Implementations;
using WeatherMonitor.Services.Interfaces;
using Xunit;

namespace WeatherMonitor_Tests
{
    public class WeatherServiceTests
    {
        [Fact]
        public void GenerateReading_CreatesAndReturnsReading()
        {
            var mockRepo = new Mock<IBaseRepository<WeatherReading>>();

            mockRepo.Setup(x => x.GetAll()).Returns(new List<WeatherReading>());

            mockRepo.Setup(x => x.Create(It.IsAny<WeatherReading>()))
                    .Returns((WeatherReading r) => r);

            var service = new WeatherService(mockRepo.Object);
            var result = service.GenerateReading();

            Assert.NotNull(result);
            
            Assert.InRange(result.Temperature, -20, 40);
        }
    }
}