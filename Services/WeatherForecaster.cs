using OpenTelemetry.Trace;

namespace TopSwagCode.Api.Services;

[RegisterService<IWeatherForecaster>(LifeTime.Scoped)]
public class WeatherForecaster : IWeatherForecaster
{
    private readonly Tracer _tracer;

    public WeatherForecaster(Tracer tracer)
    {
        _tracer = tracer;
    }
    
    private static readonly string[] SUMMARIES = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly Random _random = new();

    public async Task<WeatherForecast> GetWeatherForecastAsync(int daysFromToday, CancellationToken cancellationToken = default)
    {
        using var span = _tracer.StartActiveSpan("WeatherForecaster-Span");
        
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