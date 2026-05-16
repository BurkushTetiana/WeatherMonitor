using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WeatherMonitor.Models;
using WeatherMonitor.Repositories.Interfaces;
using WeatherMonitor.Services.Interfaces;

namespace WeatherMonitor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly IBaseRepository<WeatherReading> _repository;

        public WeatherController(IWeatherService weatherService,
                                 IBaseRepository<WeatherReading> repository)
        {
            _weatherService = weatherService;
            _repository = repository;
        }

        // GET /weather — вся історія
        [HttpGet]
        public JsonResult Get() =>
            new JsonResult(_repository.GetAll());

        // GET /weather/latest — останній запис
        [HttpGet("latest")]
        public JsonResult GetLatest()
        {
            var latest = _repository.GetAll()
                .OrderByDescending(r => r.Timestamp)
                .FirstOrDefault();
            return new JsonResult(latest);
        }

        // POST /weather — додати запис вручну
        [HttpPost]
        public JsonResult Post()
        {
            var reading = _weatherService.GenerateReading();
            return new JsonResult(reading);
        }

        // PUT /weather — оновити запис
        [HttpPut]
        public JsonResult Put([FromBody] WeatherReading reading)
        {
            bool success = true;
            WeatherReading updated = null;
            try
            {
                var existing = _repository.Get(reading.Id);
                if (existing != null)
                    updated = _repository.Update(reading);
                else
                    success = false;
            }
            catch (Exception) { success = false; }

            return success
                ? new JsonResult($"Update successful: {updated.Id}")
                : new JsonResult("Update failed");
        }

        // DELETE /weather?id=... — видалити запис
        [HttpDelete]
        public JsonResult Delete(Guid id)
        {
            bool success = true;
            try
            {
                var existing = _repository.Get(id);
                if (existing != null)
                    _repository.Delete(id);
                else
                    success = false;
            }
            catch (Exception) { success = false; }

            return success
                ? new JsonResult("Delete successful")
                : new JsonResult("Delete failed");
        }
    }
}