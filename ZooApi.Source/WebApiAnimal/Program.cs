using ApplicationAnimal.Common.Abstractions;
using ApplicationAnimal.Services.Employees.Command.CreateCommands.CreateEmployee;
using FluentValidation;
using Infrastructure;
using Infrastructure.ContextsDb;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Common.Abstractions.Employees;
using ZooApi.DTO;
using ZooApi.Mapping;
using ZooApi.Middlewares;
using ZooApi.Validations;
using ApplicationAnimal.Services.Employees.Queries;
using ApplicationAnimal.Services.Caching;
using Infrastructure.BackgroundServices;
using ApplicationAnimal.Services.Employees.Command.UpdateCommands.PaySalariesEmployee;

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

builder.WebHost.UseUrls("http://0.0.0.0:8080");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AnimalProfile));
builder.Services.AddHostedService<DecrementAnimalEnergyAsync>();
builder.Services.AddHostedService<SalaryPaymentForEmployee>();

var webApiAssembly = typeof(AddAnimalRequestDto).Assembly;
var applicationAssembly = typeof(CreateEmployeeHandler).Assembly;
var infrastructureAssembly = typeof(EmployeeRepository).Assembly;

builder.Services.AddScoped<IValidator<AddAnimalRequestDto>, AddAnimalDtoValidator>();

builder.Services.AddScoped<GetEmployeeByIdHandler>();
builder.Services.AddScoped<GetEmployeesByPositionsHandler>();
builder.Services.AddScoped<GetEmployeesHandler>(); 
builder.Services.AddScoped<GetEmployeesWithoutAnimalsHandler>();
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
builder.Services.AddScoped<PaySalariesEmployeeHandler>();

// Использование Scrutor для регистрации Application (РАСПИШИ В OBSIDIAN !) 
builder.Services.Scan(selector => selector
    .FromAssemblies(applicationAssembly)
    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime()

    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
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

var dbContext = builder.Services.AddDbContext<AppContextDB>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("AppContextDb"));
    });

var redisConnectionString = builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "ZooApi_";
});

builder.Services.AddHybridCache();

if (dbContext is null || redisConnectionString is null)
{
    Log.Fatal("Database or Redis Cache connection string is null");
    throw new InvalidOperationException("Database or Redis Cache connection string is null");
}


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
