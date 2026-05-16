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

// 1. Налаштування підключення до бази даних (SQL Server) [cite: 117]
string connection = builder.Configuration.GetConnectionString("DefaultConnection"); 
builder.Services.AddDbContext<ApplicationContext>(options =>
options.UseSqlServer(connection)); 

// 2. Реєстрація репозиторіїв та сервісів проєкту [cite: 117]
builder.Services.AddTransient<IWeatherService, WeatherService>();
builder.Services.AddTransient<IBaseRepository<WeatherReading>, BaseRepository<WeatherReading>>();

// 3. Реєстрація фонового сервісу для авто-генерації погоди щосекунди
builder.Services.AddHostedService<WeatherBackgroundService>();

builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 4. Реєстрація та налаштування політики CORS (ОБОВ'ЯЗКОВО до методу Build)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Побудова вебдодатка
var app = builder.Build();

// 5. Налаштування конвеєра обробки HTTP-запитів (Middleware) [cite: 117]
 if (app.Environment.IsDevelopment()) 
{
     app.UseDeveloperExceptionPage(); 
     app.UseSwagger();
     app.UseSwaggerUI();
}

// Дозволяємо серверу віддавати статичні файли (наш index.html з папки wwwroot)
app.UseStaticFiles();

// Активація політики CORS для запитів від браузера
app.UseCors();

 app.UseAuthorization(); 
 app.MapControllers(); 

// Запуск сервера
app.Run();