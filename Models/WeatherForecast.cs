namespace weather_forecast_service.Models;

public class WeatherForecast {
  public string Id { get; set; }
  public double Latitude { get; set; }
  public double Longitude { get; set; }
  public double Temperature { get; set; }
  public string Timestamp { get; set; }
}
