using IMedicalB.Model;
using IMedicalB.Service;
using IMedicalB.Sql;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddHttpClient<IWeatherService, WeatherService>();
builder.Services.AddScoped<ICityInfoService, CityInfoService>();
builder.Services.Configure<ApiEndpoints>(builder.Configuration.GetSection("ApiEndpoints"));
builder.Services.Configure<SqlQueries>(builder.Configuration.GetSection("SqlQueries"));
builder.Services.AddCors(options => {
    options.AddPolicy("AllowFrontend", policy => {
        policy.WithOrigins("https://localhost:5001")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
    app.UseCors("AllowFrontend");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
