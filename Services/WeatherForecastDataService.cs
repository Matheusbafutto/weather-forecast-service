using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using weather_forecast_service.Models;

namespace weather_forecast_service.Services;

public class WeatherForecastDataService {
  private readonly HttpClient client;

  private readonly string baseUrl;

  public WeatherForecastDataService(string weatherApiBaseUrl = "https://api.open-meteo.com") {
    client = new();
    client.DefaultRequestHeaders.Accept.Add(
      new MediaTypeWithQualityHeaderValue("application/json"));
    baseUrl = weatherApiBaseUrl;
  }

  public async Task<WeatherForecast?> GetForecast(double latitude, double longitude) {
    string url = String.Format(
      "{0}/v1/forecast?latitude={1}&longitude={2}&current=temperature_2m", baseUrl, latitude, longitude
    );

    var json = await client.GetStringAsync(url);

    var openMeteoForecast = JsonSerializer.Deserialize<OpenMeteoForecast>(json);

    if (openMeteoForecast == null) {
      return null;
    }

    // latitude and longitude from open-meteo response do not match input coordinates exactly
    // Assuming forecast is a proper estimation of true value on original coordinates
    return new WeatherForecast {
      Latitude = latitude,
      Longitude = longitude,
      Temperature = openMeteoForecast.current.temperature_2m,
      Timestamp = openMeteoForecast.current.time,
    };
  }
}
