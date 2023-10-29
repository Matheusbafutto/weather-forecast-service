using Microsoft.AspNetCore.Http.HttpResults;
using weather_forecast_service.Models;

namespace weather_forecast_service.Services;

public static class WeatherForecastService
{

    public static List<WeatherForecast> GetAll() => WeatherForecastInMemoryDataStoreService.GetAll();

    public static WeatherForecast? Get(double latitude, double longitude) =>
        WeatherForecastInMemoryDataStoreService.Get(latitude, longitude);

    public static WeatherForecast? Get(int id) =>
        WeatherForecastInMemoryDataStoreService.Get(id);

    public static async Task Add(double latitude, double longitude) {
        WeatherForecast? weatherForecast = await WeatherForecastDataService.getForecast(latitude, longitude);

        if (weatherForecast != null) {
            WeatherForecastInMemoryDataStoreService.Add(weatherForecast);
        }
    }

    public static void Delete(int id)
    {
        WeatherForecastInMemoryDataStoreService.Delete(id);
    }

    public static async Task<WeatherForecast?> Update(int id)
    {
        WeatherForecast? record = WeatherForecastInMemoryDataStoreService.Get(id);
        if (record == null) {
            return null;
        }

        WeatherForecast? weatherForecast = await WeatherForecastDataService.getForecast(
            record.Latitude,
            record.Longitude
        );

        if (weatherForecast != null) {
            WeatherForecastInMemoryDataStoreService.Update(weatherForecast);
            return weatherForecast;
        }
        return null;
    }
}