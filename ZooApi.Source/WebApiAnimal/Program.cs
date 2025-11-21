global using CSharpFunctionalExtensions;
using ApplicationAnimal.BackgroundServ;
using ApplicationAnimal.Common.Interfaces;
using ApplicationAnimal.Services;
using FluentValidation;
using Infrastructure.ContextsDb;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Configuration;
using Serilog;
using WebApiAnimal.Filters;
using ZooApi.DTO;
using ZooApi.Mapping;
using ZooApi.Middlewares;
using ZooApi.Validations;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    });

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
});

//builder.WebHost.UseUrls("http://0.0.0.0:8080"); // - хост, использующийся в Docker. Также для запуска с Docker, необходимо в appsettings.json заменить localhost на postgre_db
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AnimalProfile));
builder.Services.AddMemoryCache();
builder.Services.AddHostedService<DecrementAnimalEnergyAsync>();

builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IValidator<AddAnimalRequestDto>, AddAnimalDtoValidator>();
builder.Services.AddScoped<CacheAttribute>();

builder.Services.AddDbContext<AppContextDB>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("AppContextDb"));
    });

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppContextDB>();
await db.Database.MigrateAsync();

Log.Information("Application ZooApi starting up");

app.UseExceptionHandlingMiddleware();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseRouting();
app.UseHttpLogging();
app.MapControllers();
app.Run();
