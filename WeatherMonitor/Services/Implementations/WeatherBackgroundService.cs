using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherMonitor.Services.Interfaces;

namespace WeatherMonitor.Services.Implementations
{
    public class WeatherBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public WeatherBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var weatherService = scope.ServiceProvider
                    .GetRequiredService<IWeatherService>();
                weatherService.GenerateReading();

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}