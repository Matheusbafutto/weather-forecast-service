using weather_forecast_service.Models;

namespace weather_forecast_service.Interfaces;

public interface IWeatherForecastDataClient {
  public Task<WeatherForecast?> GetForecast(double latitude, double longitude);
}
