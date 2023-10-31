using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using weather_forecast_service.Controllers;
using weather_forecast_service.Interfaces;
using weather_forecast_service.Models;
using weather_forecast_service.Services;
using Xunit;

// TODO: imporve test coverage
public class WeatherForecastControllerTests {
  [Fact]
  public async Task Return_Forecast_History_When_Calling_Getall() {
    var mockClient = new Mock<HttpClient>();

    IWeatherForecastDataClient weatherForecastDataService = new WeatherForecastDataService(
      mockClient.Object,
      "https://dummy.open-meteo.com"
    );

    var mockWeatherForecastDataStore = new Mock<IWeatherForecastDataStore>();
    mockWeatherForecastDataStore.Setup<Task<WeatherForecast?[]>>(_ => _.GetAll()).ReturnsAsync(
      new WeatherForecast[] {
        new WeatherForecast {
          Id = "123",
          Latitude = 100000,
          Longitude = 100000,
          Temperature = 27.2,
          Timestamp = "10/10/2023"
        },
      }
    );

    WeatherForecastService weatherForecastService = new(mockWeatherForecastDataStore.Object, weatherForecastDataService);

    WeatherForecastController controller = new(weatherForecastService);

    var result = await controller.Get();

    Assert.Equal(1, result.Length);
  }

  [Fact]
  public async Task Returns_One_Existing_Forecast_On_Get_By_Id() {
    var mockClient = new Mock<HttpClient>();

    IWeatherForecastDataClient weatherForecastDataService = new WeatherForecastDataService(
      mockClient.Object,
      "https://dummy.open-meteo.com"
    );

    var mockWeatherForecastDataStore = new Mock<IWeatherForecastDataStore>();
    mockWeatherForecastDataStore.Setup<Task<WeatherForecast?>>(_ => _.Get("123")).ReturnsAsync(
        new WeatherForecast {
          Id = "123",
          Latitude = 100000,
          Longitude = 100000,
          Temperature = 27.2,
          Timestamp = "10/10/2023"
        }
    );

    WeatherForecastService weatherForecastService = new(mockWeatherForecastDataStore.Object, weatherForecastDataService);

    WeatherForecastController controller = new(weatherForecastService);

    var result = await controller.Get("123");

    Assert.NotNull(result);
    Assert.IsType<ActionResult<WeatherForecast>>(result);
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

    var result = await controller.Get("notvalid");

    Assert.NotNull(result);
    Assert.IsType<ActionResult<WeatherForecast>>(result);
  }
}
