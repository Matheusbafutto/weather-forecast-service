using weather_forecast_service.Interfaces;
using weather_forecast_service.Models;

namespace weather_forecast_service.Services;

public class WeatherForecastService {

    private readonly IWeatherForecastDataStore _weatherForecastDataStore;

    private readonly IWeatherForecastDataClient _weatherForecastDataService;

    public WeatherForecastService(
        IWeatherForecastDataStore weatherForecastDataStore,
        IWeatherForecastDataClient weatherForecastDataService
    ) {
        _weatherForecastDataStore = weatherForecastDataStore;
        _weatherForecastDataService = weatherForecastDataService;
    }

  public Task<WeatherForecast?[]> GetAll() {
    return _weatherForecastDataStore.GetAll();
  }

  public Task<WeatherForecast?> Get(double latitude, double longitude) {
        return _weatherForecastDataStore.Get(latitude, longitude);
    }

    public Task<WeatherForecast?> Get(string id) {
        return _weatherForecastDataStore.Get(id);
    }

    public async Task Add(double latitude, double longitude) {
        WeatherForecast? weatherForecast = await _weatherForecastDataService.GetForecast(latitude, longitude);

        if (weatherForecast != null) {
            await _weatherForecastDataStore.Add(weatherForecast);
        }
    }

    public Task Delete(string id) {
        return _weatherForecastDataStore.Delete(id);
    }

    public async Task<WeatherForecast?> Update(string id) {
        WeatherForecast? record = await _weatherForecastDataStore.Get(id);
        if (record == null) {
            return null;
        }

        WeatherForecast? weatherForecast = await _weatherForecastDataService.GetForecast(
            record.Latitude,
            record.Longitude
        );

        if (weatherForecast != null) {
            weatherForecast.Id = record.Id;
            await _weatherForecastDataStore.Update(weatherForecast);
            return weatherForecast;
        }
        return null;
    }
}