using Microsoft.OpenApi.Models;
using SportsNewsAPI.Models;
using SportsNewsAPI.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<SportsNewsDatabaseSettings>(
    builder.Configuration.GetSection("SportsNewsDatabase"));

builder.Services.AddSingleton<SportsNewsService>();
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Настройка сериализации DateOnly в ISO формат (yyyy-MM-dd)
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sports News API",
        Version = "v1",
        Description = "API для управления спортивными новостями"
    });

    // Добавляем поддержку DateOnly в Swagger
    c.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date",
        Example = new Microsoft.OpenApi.Any.OpenApiString("2024-03-04")
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }

// "C:\Program Files\MongoDB\Server\8.2\bin\mongod.exe" --dbpath C:\Users\User\Desktop\3COURSE\SOTR\SportNewsDB