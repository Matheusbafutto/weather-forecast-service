using Microsoft.AspNetCore.Mvc;
using weather_forecast_service.Models;
using weather_forecast_service.Services;

namespace weather_forecast_service.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    // GET bulk read of existing records in the system
    [HttpGet("all")]
    public IEnumerable<WeatherForecast> Get()
    {
        return WeatherForecastService.GetAll();
    }

    // GET reads forecast record by location
    [HttpGet("location")]
    public ActionResult<WeatherForecast> Get([FromQuery] double latitude, [FromQuery] double longitude) {
        var result = WeatherForecastService.Get(latitude, longitude);

        if (result == null) {
            return NotFound();
        }

        return result;
    }

    // GET reads forecast record by id
    [HttpGet()]
    public ActionResult<WeatherForecast> Get([FromQuery] int id) {
        var result = WeatherForecastService.Get(id);

        if (result == null) {
            return NotFound();
        }

        return result;
    }

    // POST creates a new forecast record in the system with the latest forecast
    [HttpPost()]
    public async Task<ActionResult> Post([FromQuery] double latitude, [FromQuery] double longitude) {
        WeatherForecast? result = WeatherForecastService.Get(latitude, longitude);
        if (result != null) {
            return Conflict(result);
        }

        await WeatherForecastService.Add(latitude, longitude);
        return Ok();
    }

    // PUT is meant to be used to refresh the forecast for an existing record
    [HttpPut()]
    public async Task<ActionResult> Put([FromQuery] int id) {
        WeatherForecast? result = WeatherForecastService.Get(id);
        if (result == null) {
            return NotFound();
        }

        await WeatherForecastService.Update(id);
        return Ok();
    }

    // DELETE removes specified record from the system if it exists
    [HttpDelete()]
    public ActionResult Delete([FromQuery] int id) {
        WeatherForecast? result = WeatherForecastService.Get(id);

        if (result == null) {
            return NotFound();
        }
        WeatherForecastService.Delete(id);
        return NoContent();
    }
}
