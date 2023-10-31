using System.Net.Http.Headers;
using System.Text.Json;
using weather_forecast_service.Interfaces;
using weather_forecast_service.Models;

namespace weather_forecast_service.Services;

public class WeatherForecastDataService : IWeatherForecastDataClient {
  private readonly HttpClient client;

  private readonly string baseUrl;

  public WeatherForecastDataService(
    HttpClient _client,
    string weatherApiBaseUrl = "https://api.open-meteo.com"
  ) {
    client = _client;
    client.DefaultRequestHeaders.Accept.Add(
      new MediaTypeWithQualityHeaderValue("application/json"));
    baseUrl = weatherApiBaseUrl;
  }

  public async Task<WeatherForecast?> GetForecast(double latitude, double longitude) {
    string url = String.Format(
      "{0}/v1/forecast?latitude={1}&longitude={2}&current=temperature_2m", baseUrl, latitude, longitude
    );

    var json = await client.GetStringAsync(url);
    ValidatorService.ValidateOpenMeteo(json);

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
