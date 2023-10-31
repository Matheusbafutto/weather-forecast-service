using MongoDB.Driver;
using weather_forecast_service.Interfaces;
using weather_forecast_service.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("MONGODB_URI");
if (connectionString == null) {
    Console.WriteLine("You must set your 'MONGODB_URI' environment variable. To learn how to set it, see https://www.mongodb.com/docs/drivers/csharp/current/quick-start/#set-your-connection-string");
    Environment.Exit(0);
}
IMongoClient mongoClient = new MongoClient(connectionString);

// Add services to the container.
builder.Services.AddSingleton(mongoClient);
builder.Services.AddSingleton<IWeatherForecastDataClient, WeatherForecastDataService>();
builder.Services.AddSingleton<IWeatherForecastDataStore, WeatherForecastMongoDataStoreService>();
builder.Services.AddSingleton<WeatherForecastService, WeatherForecastService>();
builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
