using Microsoft.EntityFrameworkCore;
using WeatherMonitor.Models;

namespace WeatherMonitor.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<WeatherReading> WeatherReadings { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
          
        }
    }
}