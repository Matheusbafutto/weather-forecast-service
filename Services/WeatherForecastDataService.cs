using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using weather_forecast_service.Models;

namespace weather_forecast_service.Services;

public static class WeatherForecastDataService {
  private static HttpClient client { get; }

  private static String baseUrl { get; } = "https://api.open-meteo.com";

  static WeatherForecastDataService() {
    client = new();
    client.DefaultRequestHeaders.Accept.Add(
      new MediaTypeWithQualityHeaderValue("application/json"));
  }

  public static async Task<WeatherForecast?> getForecast(double latitude, double longitude) {
    String url = String.Format(
      "{0}/v1/forecast?latitude={1}&longitude={2}&current=temperature_2m", baseUrl, latitude, longitude
    );

    var json = await client.GetStringAsync(url);

    var openMeteoForecast = JsonSerializer.Deserialize<OpenMeteoForecast>(json);

    if (openMeteoForecast == null) {
      return null;
    }

    return new WeatherForecast {
      Latitude = openMeteoForecast.latitude,
      Longitude = openMeteoForecast.longitude,
      Temperature = openMeteoForecast.current.temperature_2m,
      Timestamp = openMeteoForecast.current.time,
    };
  }
}
