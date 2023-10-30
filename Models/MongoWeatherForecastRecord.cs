using MongoDB.Bson;

namespace weather_forecast_service.Models;

public class MongoWeatherForecastRecord {
  public ObjectId Id { get; set; }
  public int Latitude { get; set; }
  public int Longitude { get; set; }
  public double Temperature { get; set; }
  public string Timestamp { get; set; }
};

public class MongoWeatherForecastNewEntry {
  public int Latitude { get; set; }
  public int Longitude { get; set; }
  public double Temperature { get; set; }
  public string Timestamp { get; set; }
};
