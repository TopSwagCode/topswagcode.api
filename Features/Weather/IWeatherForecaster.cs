namespace TopSwagCode.Api.Features.Weather;

public interface IWeatherForecaster
{
    Task<WeatherForecast> GetWeatherForecastAsync(int daysFromToday, CancellationToken cancellationToken = default);
}