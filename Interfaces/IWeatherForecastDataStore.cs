using weather_forecast_service.Models;

namespace weather_forecast_service.Interfaces;

public interface IWeatherForecastDataStore {
  Task<WeatherForecast?[]> GetAll();

  Task<WeatherForecast?> Get(string id);

  Task<WeatherForecast?> Get(double latitude, double longitude);

  Task Add(WeatherForecast weatherForecast);

  Task Delete(string id);

  Task Update(WeatherForecast weatherForecast);
}
