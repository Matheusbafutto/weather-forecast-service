using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace weather_forecast_service.Services;

class ValidationException : Exception {
  public readonly IList<string> errors;

  public ValidationException(string message, IList<string> _errors) : base(message) {
    errors = _errors;
  }
}

class OpenMeteoValidationException : ValidationException {
  public OpenMeteoValidationException(IList<string> errors) : base("OpenMeteoValidationException", errors) {}
}

public static class ValidatorService {
  public static void ValidateOpenMeteo(string json) {
    JSchema schema = JSchema.Parse(System.IO.File.ReadAllText("./Services/Validators/schemas/open-meteo-json-schema.json"));
    JObject jsonObject = JObject.Parse(json);
    IList<string> validationErrors = new List<string>();

    if (jsonObject.IsValid(schema, out validationErrors) == false) {
      throw new OpenMeteoValidationException(validationErrors);
    }
  }
}
