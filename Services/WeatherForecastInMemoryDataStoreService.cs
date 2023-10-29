using weather_forecast_service.Models;

namespace weather_forecast_service.Services;

public static class WeatherForecastInMemoryDataStoreService
{
    static List<WeatherForecast> WeatherForecasts { get; }
    static int nextId = 1;
    static WeatherForecastInMemoryDataStoreService()
    {
        WeatherForecasts = new List<WeatherForecast> {};
    }

    public static List<WeatherForecast> GetAll() => WeatherForecasts;

    public static WeatherForecast? Get(double latitude, double longitude) =>
        WeatherForecasts.FirstOrDefault(wf => wf.Latitude == latitude & wf.Longitude == longitude);

    public static WeatherForecast? Get(int id) =>
        WeatherForecasts.FirstOrDefault(wf => wf.Id == id);

    public static void Add(WeatherForecast weatherForecast)
    {
        weatherForecast.Id = nextId++;
        WeatherForecasts.Add(weatherForecast);
    }

    public static void Delete(int id)
    {
        var weatherForecast = Get(id);
        if(weatherForecast is null)
            return;

        WeatherForecasts.Remove(weatherForecast);
    }

    public static void Update(WeatherForecast weatherForecast)
    {
        var index = WeatherForecasts.FindIndex(p => p.Id == weatherForecast.Id);
        if(index == -1)
            return;

        WeatherForecasts[index] = weatherForecast;
    }
}