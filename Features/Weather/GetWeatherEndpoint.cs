namespace TopSwagCode.Api.Features.Weather;

public class GetWeatherEndpoint : EndpointWithoutRequest
{
    private readonly IWeatherForecaster _weatherForecaster;

    public GetWeatherEndpoint(IWeatherForecaster weatherForecaster)
    {
        _weatherForecaster = weatherForecaster;
    }
    
    public override void Configure()
    {
        Get("/weatherforecast");
        Throttle(
            hitLimit: 5,
            durationSeconds: 10,
            headerName: "X-Client-Id" // this is optional
        );
        AllowAnonymous();
    }
    
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(streamWeatherForecastsAsync());
    }
    
    async IAsyncEnumerable<WeatherForecast> streamWeatherForecastsAsync()
    {
        for (int daysFromToday = 1; daysFromToday <= 50; daysFromToday++)
        {
            WeatherForecast weatherForecast = await _weatherForecaster.GetWeatherForecastAsync(daysFromToday);

            yield return weatherForecast;
        };
    }  
}