using Microsoft.AspNetCore.SignalR.Protocol;
using MongoDB.Driver;
using weather_forecast_service.Interfaces;
using weather_forecast_service.Models;

namespace weather_forecast_service.Services;

public class WeatherForecastMongoDataStoreService : IWeatherForecastDataStore {

  private readonly IMongoClient client;
  private readonly IMongoCollection<MongoWeatherForecastRecord> forecastCollection;
  private readonly IMongoCollection<MongoWeatherForecastNewEntry> forecastCollectionNewEntry;

  public WeatherForecastMongoDataStoreService(IMongoClient _client) {
    client = _client;
    forecastCollection = client.GetDatabase("weatherForecasts").GetCollection<MongoWeatherForecastRecord>("forecasts");
    forecastCollectionNewEntry = client.GetDatabase("weatherForecasts").GetCollection<MongoWeatherForecastNewEntry>("forecasts");
  }

  public async Task<WeatherForecast?[]> GetAll() {
    var filter = Builders<MongoWeatherForecastRecord>.Filter.Empty;
    List<MongoWeatherForecastRecord> records = await forecastCollection.Find(filter).ToListAsync();

    WeatherForecast?[] results = new WeatherForecast?[records.Count];
    for (var i = 0; i < records.Count; i++)
    {
       results[i] = mapMongoRecordToWeatherForecast(records[i]);
    }
    return  results;
  }

  public async Task<WeatherForecast?> Get(string id) {
    var filter = Builders<MongoWeatherForecastRecord>.Filter.Eq(r => r.Id, new MongoDB.Bson.ObjectId(id));
    MongoWeatherForecastRecord? result = await forecastCollection.Find(filter).FirstOrDefaultAsync();
    return mapMongoRecordToWeatherForecast(result);
  }

  public async Task<WeatherForecast?> Get(double latitude, double longitude) {
    var filterLat = Builders<MongoWeatherForecastRecord>.Filter.Eq(r => r.Latitude, (int)(latitude * Math.Pow(10, 4)));
    var filterLong = Builders<MongoWeatherForecastRecord>.Filter.Eq(r => r.Longitude, (int)(longitude * Math.Pow(10, 4)));
    MongoWeatherForecastRecord? result = await forecastCollection.Find(filterLat & filterLong).FirstOrDefaultAsync();
    return mapMongoRecordToWeatherForecast(result);
  }

  public async Task Add(WeatherForecast weatherForecast) {
    // We use coordinates for equality comparison in this file's Get(double latitude, double longitude) method
    // to avoid issues with approximation, storing floats as integers
    MongoWeatherForecastNewEntry record = new() {
      Latitude = (int)(weatherForecast.Latitude * Math.Pow(10, 4)),
      Longitude = (int)(weatherForecast.Longitude * Math.Pow(10, 4)),
      Temperature = weatherForecast.Temperature,
      Timestamp = weatherForecast.Timestamp,
    };
    await forecastCollectionNewEntry.InsertOneAsync(record);
  }

  public async Task Delete(string id) {
    var filter = Builders<MongoWeatherForecastRecord>.Filter.Eq(r => r.Id, new MongoDB.Bson.ObjectId(id));
    await forecastCollection.DeleteOneAsync(filter);
  }

  public async Task Update(WeatherForecast weatherForecast) {
    MongoWeatherForecastRecord record = mapWeatherForecastToMongoRecord(weatherForecast);
    var filter = Builders<MongoWeatherForecastRecord>.Filter.Eq(r => r.Id, record.Id);
    await forecastCollection.FindOneAndReplaceAsync<MongoWeatherForecastRecord>(filter, record);
  }

  private MongoWeatherForecastRecord mapWeatherForecastToMongoRecord(WeatherForecast weatherForecast) {
    // We use coordinates for equality comparison in this file's Get(double latitude, double longitude) method
    // to avoid issues with approximation, storing floats as integers
    MongoWeatherForecastRecord record =  new() {
      Latitude = (int)(weatherForecast.Latitude * Math.Pow(10, 4)),
      Longitude = (int)(weatherForecast.Longitude * Math.Pow(10, 4)),
      Temperature = weatherForecast.Temperature,
      Timestamp = weatherForecast.Timestamp,
    };

    if (weatherForecast.Id != null) {
      record.Id = new MongoDB.Bson.ObjectId(weatherForecast.Id);
    }
    return record;
  }

  private WeatherForecast? mapMongoRecordToWeatherForecast(MongoWeatherForecastRecord? mongoWeatherForecastRecord) {
    if (mongoWeatherForecastRecord == null) {
      return null;
    }

    // because we multiply float coordinates by 10^4 an cast them as integers
    // we need to revert them back to approximated values when outputting from data store
    WeatherForecast forecast = new() {
      Id = mongoWeatherForecastRecord.Id.ToString(),
      Latitude = (double)(mongoWeatherForecastRecord.Latitude) / Math.Pow(10, 4),
      Longitude = (double)(mongoWeatherForecastRecord.Longitude) / Math.Pow(10, 4),
      Temperature = mongoWeatherForecastRecord.Temperature,
      Timestamp = mongoWeatherForecastRecord.Timestamp,
    };

    return forecast;
  }
}
