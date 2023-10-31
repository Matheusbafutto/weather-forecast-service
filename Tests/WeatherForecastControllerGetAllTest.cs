using Moq;
using weather_forecast_service.Controllers;
using weather_forecast_service.Interfaces;
using weather_forecast_service.Models;
using weather_forecast_service.Services;
using Xunit;

// TODO: imporve test coverage
public class WeatherForecastControllerGetAllTest {
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
          Latitude = 10,
          Longitude = 10,
          Temperature = 27.2,
          Timestamp = "10/10/2023"
        },
      }
    );

    WeatherForecastService weatherForecastService = new(mockWeatherForecastDataStore.Object, weatherForecastDataService);

    WeatherForecastController controller = new(weatherForecastService);

    var result = await controller.Get();

    Assert.True(result.Length == 1);
  }
}
