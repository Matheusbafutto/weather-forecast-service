using System.Net;
using Microsoft.AspNetCore.Mvc;
using weather_forecast_service.Models;
using weather_forecast_service.Services;

namespace weather_forecast_service.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    private readonly WeatherForecastService _weatherForecastService;

    public WeatherForecastController(WeatherForecastService weatherForecastService) {
        _weatherForecastService = weatherForecastService;
    }

    // GET bulk read of existing records in the system
    [HttpGet("all")]
    public Task<WeatherForecast?[]> Get()
    {
        return _weatherForecastService.GetAll();
    }

    // GET reads forecast record by location
    [HttpGet("location")]
    public async Task<ActionResult<WeatherForecast>> Get([FromQuery] double latitude, [FromQuery] double longitude) {  
        var result = await _weatherForecastService.Get(latitude, longitude);

        if (result == null) {
            return NotFound();
        }

        return result;
    }

    // GET reads forecast record by id
    [HttpGet()]
    public async Task<ActionResult<WeatherForecast>> Get([FromQuery] string id) {
        var result = await _weatherForecastService.Get(id);

        if (result == null) {
            return NotFound();
        }

        return result;
    }

    // POST creates a new forecast record in the system with the latest forecast
    [HttpPost()]
    public async Task<ActionResult> Post([FromQuery] double latitude, [FromQuery] double longitude) {
        WeatherForecast? result = await _weatherForecastService.Get(latitude, longitude);
        if (result != null) {
            return Conflict(result);
        }

        try {
            await _weatherForecastService.Add(latitude, longitude);
        } catch (Exception e) {
            Console.WriteLine(
                string.Format(
                    "Exception '{0}' caught in WeatherForecastController Post(latitude={1}, longitude={2})",
                    e.Message, latitude, longitude
                )
            );
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
        result = await _weatherForecastService.Get(latitude, longitude);
        return Ok(result);
    }

    // PUT is meant to be used to refresh the forecast for an existing record
    [HttpPut()]
    public async Task<ActionResult> Put([FromQuery] string id) {
        WeatherForecast? result = await _weatherForecastService.Get(id);
        if (result == null) {
            return NotFound();
        }

        try {
            await _weatherForecastService.Update(id);
        } catch (Exception e) {
            Console.WriteLine(
                string.Format(
                    "Exception '{0}' caught in WeatherForecastController Put(id={1})",
                    e.Message, id
                )
            );
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
        result = await _weatherForecastService.Get(id);
        return Ok(result);
    }

    // DELETE removes specified record from the system if it exists
    [HttpDelete()]
    public async Task<ActionResult> Delete([FromQuery] string id) {
        WeatherForecast? result = await _weatherForecastService.Get(id);

        if (result == null) {
            return NotFound();
        }
        await _weatherForecastService.Delete(id);
        return NoContent();
    }
}
