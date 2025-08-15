using DataAccess;
using FluentValidation;
using LibraryAnimals;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddHttpLogging(logging => {
    logging.LoggingFields = HttpLoggingFields.All;
});

builder.Logging
    .ClearProviders()
    .AddConsole()
    .SetMinimumLevel(LogLevel.Debug);

//builder.WebHost.UseUrls("http://0.0.0.0:8080"); - для Docker. Также для запуска с Docker, необходимо в appsettings.json замеить localhost на postgre_db
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLibraryAnimals();
builder.Services.AddAutoMapper(typeof(AnimalProfile));

builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IValidator<CreateAnimalDto>, CreateAnimalDtoValidator>();

builder.Services.AddDbContext<AppContextDB>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("AppContextDb"));
    });

var app = builder.Build();
app.UseExceptionHandlingMiddleware();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppContextDB>();
await db.Database.MigrateAsync();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseRouting();
app.UseHttpLogging();
app.MapControllers();
app.Run();
