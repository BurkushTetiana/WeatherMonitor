using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WeatherMonitor.Models;
using WeatherMonitor.Controllers;
using WeatherMonitor.Repositories.Interfaces;
using WeatherMonitor.Services.Interfaces;

namespace WeatherMonitor_Tests
{
    public class WeatherControllerTests
    {
        private WeatherReading GetReading() => new WeatherReading
        {
            Id = Guid.NewGuid(),
            Temperature = 22.5,
            Humidity = 60.0,
            Pressure = 1013.0,
            Timestamp = DateTime.UtcNow
        };

        [Fact]
        public void Get_ReturnsData()
        {
            var mockRepo = new Mock<IBaseRepository<WeatherReading>>();
            var mockService = new Mock<IWeatherService>();
            var reading = GetReading();
            mockRepo.Setup(x => x.GetAll())
                    .Returns(new List<WeatherReading> { reading });

            var controller = new WeatherController(mockService.Object, mockRepo.Object);
            var result = controller.Get() as JsonResult;

            Assert.Equal(new List<WeatherReading> { reading }, result.Value);
        }

        [Fact]
        public void Get_NotNull()
        {
            var mockRepo = new Mock<IBaseRepository<WeatherReading>>();
            var mockService = new Mock<IWeatherService>();
            mockRepo.Setup(x => x.GetAll()).Returns(new List<WeatherReading>());

            var controller = new WeatherController(mockService.Object, mockRepo.Object);
            var result = controller.Get() as JsonResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void Post_ReturnsReading()
        {
            var mockRepo = new Mock<IBaseRepository<WeatherReading>>();
            var mockService = new Mock<IWeatherService>();
            var reading = GetReading();
            mockService.Setup(x => x.GenerateReading()).Returns(reading);

            var controller = new WeatherController(mockService.Object, mockRepo.Object);
            var result = controller.Post() as JsonResult;

            Assert.Equal(reading, result.Value);
        }

        [Fact]
        public void Put_UpdatesCorrectly()
        {
            var mockRepo = new Mock<IBaseRepository<WeatherReading>>();
            var mockService = new Mock<IWeatherService>();
            var reading = GetReading();
            mockRepo.Setup(x => x.Get(reading.Id)).Returns(reading);
            mockRepo.Setup(x => x.Update(reading)).Returns(reading);

            var controller = new WeatherController(mockService.Object, mockRepo.Object);
            var result = controller.Put(reading) as JsonResult;

            Assert.Equal($"Update successful: {reading.Id}", result.Value);
        }

        [Fact]
        public void Delete_RemovesCorrectly()
        {
            var mockRepo = new Mock<IBaseRepository<WeatherReading>>();
            var mockService = new Mock<IWeatherService>();
            var reading = GetReading();
            mockRepo.Setup(x => x.Get(reading.Id)).Returns(reading);
            mockRepo.Setup(x => x.Delete(reading.Id));

            var controller = new WeatherController(mockService.Object, mockRepo.Object);
            var result = controller.Delete(reading.Id) as JsonResult;

            Assert.Equal("Delete successful", result.Value);
        }
    }
}