namespace TopSwagCode.Api.Services;

public interface IWeatherForecaster
{
    Task<WeatherForecast> GetWeatherForecastAsync(int daysFromToday, CancellationToken cancellationToken = default);
}