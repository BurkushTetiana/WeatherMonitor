using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WeatherMonitor.Database;
using WeatherMonitor.Models;
using WeatherMonitor.Repositories.Implementations;
using WeatherMonitor.Repositories.Interfaces;
using WeatherMonitor.Services.Implementations;
using WeatherMonitor.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection"); 
builder.Services.AddDbContext<ApplicationContext>(options =>
options.UseSqlServer(connection)); 

builder.Services.AddTransient<IWeatherService, WeatherService>();
builder.Services.AddTransient<IBaseRepository<WeatherReading>, BaseRepository<WeatherReading>>();

builder.Services.AddHostedService<WeatherBackgroundService>();

builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

 if (app.Environment.IsDevelopment()) 
{
     app.UseDeveloperExceptionPage(); 
     app.UseSwagger();
     app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseCors();

 app.UseAuthorization(); 
 app.MapControllers(); 

app.Run();