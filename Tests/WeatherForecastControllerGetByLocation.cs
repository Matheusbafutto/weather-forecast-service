using Microsoft.AspNetCore.Mvc;
using Moq;
using weather_forecast_service.Controllers;
using weather_forecast_service.Interfaces;
using weather_forecast_service.Models;
using weather_forecast_service.Services;
using Xunit;

// TODO: imporve test coverage
public class WeatherForecastControllerGetByLocation {
  [Fact]
  public async Task Returns_One_Existing_Forecast_On_Get_By_Location() {
    var mockClient = new Mock<HttpClient>();

    IWeatherForecastDataClient weatherForecastDataService = new WeatherForecastDataService(
      mockClient.Object,
      "https://dummy.open-meteo.com"
    );

    var mockWeatherForecastDataStore = new Mock<IWeatherForecastDataStore>();
    mockWeatherForecastDataStore.Setup<Task<WeatherForecast?>>(_ => _.Get(10.2, 10)).ReturnsAsync(
        new WeatherForecast {
          Id = "123",
          Latitude = 10.2,
          Longitude = 10,
          Temperature = 27.2,
          Timestamp = "10/10/2023"
        }
    );

    WeatherForecastService weatherForecastService = new(mockWeatherForecastDataStore.Object, weatherForecastDataService);

    WeatherForecastController controller = new(weatherForecastService);

    var result = await controller.Get(10.2, 10);

    Assert.NotNull(result.Value);
    Assert.IsType<ActionResult<WeatherForecast>>(result);
    Assert.True(result.Value.Latitude == 10.2);
    Assert.True(result.Value.Longitude == 10);

  }

  [Fact]
  public async Task Returns_NotFound_For_Invalid_Records() {
    var mockClient = new Mock<HttpClient>();

    IWeatherForecastDataClient weatherForecastDataService = new WeatherForecastDataService(
      mockClient.Object,
      "https://dummy.open-meteo.com"
    );

    var mockWeatherForecastDataStore = new Mock<IWeatherForecastDataStore>();

    WeatherForecastService weatherForecastService = new(mockWeatherForecastDataStore.Object, weatherForecastDataService);

    WeatherForecastController controller = new(weatherForecastService);

    var result = await controller.Get(10000, 10000);

    Assert.Null(result.Value);
  }
}
