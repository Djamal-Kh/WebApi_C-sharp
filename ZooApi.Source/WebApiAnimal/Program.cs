global using CSharpFunctionalExtensions;
using ApplicationAnimal.BackgroundServ;
using ApplicationAnimal.Common.Abstractions;
using ApplicationAnimal.Common.DTO;
using ApplicationAnimal.Services.Animals;
using ApplicationAnimal.Services.Employees;
using ApplicationAnimal.Services.Employees.Command.CreateCommands.CreateBoundWithAnimal;
using ApplicationAnimal.Services.Employees.Command.CreateCommands.CreateEmployee;
using ApplicationAnimal.Services.Employees.Command.DeleteCommands.DeleteEmployee;
using ApplicationAnimal.Services.Employees.Command.DeleteCommands.RemoveAllBoundAnimals;
using ApplicationAnimal.Services.Employees.Command.UpdateCommands.DemoteEmployee;
using ApplicationAnimal.Services.Employees.Command.UpdateCommands.PromoteEmployee;
using FluentValidation;
using Infrastructure;
using Infrastructure.ContextsDb;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Configuration;
using Serilog;
using Shared.Common.Abstractions.Employees;
using WebApiAnimal.Filters;
using ZooApi.DTO;
using ZooApi.Mapping;
using ZooApi.Middlewares;
using ZooApi.Validations;
using Scrutor;
using System.Reflection;
using ApplicationAnimal.Services.Employees.Queries;
using Npgsql.EntityFrameworkCore.PostgreSQL;


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
builder.Services.AddScoped<CacheAttribute>();

var webApiAssembly = typeof(AddAnimalRequestDto).Assembly;
var applicationAssembly = typeof(CreateEmployeeHandler).Assembly;
var infrastructureAssembly = typeof(EmployeeRepository).Assembly;

builder.Services.AddScoped<GetEmployeeByIdHandler>();
builder.Services.AddScoped<GetEmployeesByPositionsHandler>();
builder.Services.AddScoped<GetEmployeesHandler>();
builder.Services.AddScoped<GetEmployeesWithoutAnimalsHandler>();

// Использование Scrutor для регистрации Application (РАСПИШИ В OBSIDIAN !) 
builder.Services.Scan(selector => selector
    .FromAssemblies(applicationAssembly)
    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime()

    .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Service")))
    .AsImplementedInterfaces()
    .WithScopedLifetime()

    .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

    
// Использование Scrutor для регистрации Infrastructure (РАСПИШИ В OBSIDIAN !)
builder.Services.Scan(selector => selector
    .FromAssemblies(infrastructureAssembly)
    .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Repository")))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// Использование Scrutor для регистрации WebApi (РАСПИШИ В OBSIDIAN !)
builder.Services.Scan(selector => selector
    .FromAssemblies(webApiAssembly)
    .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime()

    .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Service")))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.AddSingleton<IDbConnectionFactory, NpsqlConnectionFactory>();

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
