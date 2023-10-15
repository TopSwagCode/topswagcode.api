using System.Globalization;
using System.Net;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;

namespace TopSwagCode.Api.Features.Weather;

public class GetWeatherEndpointV2 : EndpointWithoutRequest
{
    private readonly IWeatherForecaster _weatherForecaster;

    public GetWeatherEndpointV2(IWeatherForecaster weatherForecaster)
    {
        _weatherForecaster = weatherForecaster;
    }
    
    public override void Configure()
    {
        Get("/api/weatherforecast/v2");
        Throttle(
            hitLimit: 5,
            durationSeconds: 10,
            headerName: "X-Client-Id" // this is optional
        );
        AllowAnonymous();
    }
    
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendEventStreamAsync("test", streamWeatherForecastsAsync());
    }
    
    async IAsyncEnumerable<WeatherForecast> streamWeatherForecastsAsync() // Handle cancellation
    {
        for (int daysFromToday = 1; daysFromToday <= 50; daysFromToday++)
        {
            WeatherForecast weatherForecast = await _weatherForecaster.GetWeatherForecastAsync(daysFromToday);

            yield return weatherForecast;
        };
    }  
}