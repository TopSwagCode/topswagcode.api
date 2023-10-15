namespace TopSwagCode.Api.Features.Weather;

[RegisterService<IWeatherForecaster>(LifeTime.Scoped)]
public class WeatherForecaster : IWeatherForecaster
{
    private static readonly string[] SUMMARIES = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly Random _random = new();

    public async Task<WeatherForecast> GetWeatherForecastAsync(int daysFromToday, CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);

        return new WeatherForecast
        {
            DateFormatted = DateTime.Now.AddDays(daysFromToday).ToString("d"),
            TemperatureC = _random.Next(-20, 55),
            Summary = SUMMARIES[_random.Next(SUMMARIES.Length)]
        };
    }
}

public class WeatherForecast
{
    public string? DateFormatted { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}