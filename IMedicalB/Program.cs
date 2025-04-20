using System.Threading.RateLimiting;
using IMedicalB.Model;
using IMedicalB.Service;
using IMedicalB.Sql;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("fixed", context =>
    {
        var remoteIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.Get(remoteIp, _ =>
        {
            var limiterOptions = new FixedWindowRateLimiterOptions
            {
                PermitLimit = 30, // Máximo 5 solicitudes
                Window = TimeSpan.FromSeconds(60), // Cada 10 segundos
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            };

            return new FixedWindowRateLimiter(limiterOptions); 
        });
    });
});

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

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();
