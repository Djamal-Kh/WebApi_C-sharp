using LibraryAnimals;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLibraryAnimals();

builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IAnimalService, AnimalService>();

builder.Services.AddDbContext<AppContextDB>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("AppContextDb"));
    });

var app = builder.Build();


using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppContextDB>();
await db.Database.MigrateAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
