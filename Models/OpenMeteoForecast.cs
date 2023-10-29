namespace weather_forecast_service.Models;

public class OpenMeteoCurrentRecord {
  public String time { get; set; }

  public double temperature_2m { get; set; }
};

public class OpenMeteoForecast {

  public double latitude { get; set; }

  public double longitude { get; set; }

  public OpenMeteoCurrentRecord current { get; set; }
};
