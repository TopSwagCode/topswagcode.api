using Microsoft.AspNetCore.SignalR;
using TopSwagCode.Api.Services;

namespace TopSwagCode.Api.Features.Weather;

public class WeatherHub : Hub
{
    private readonly IWeatherForecaster _weatherForecaster;

    public WeatherHub(IWeatherForecaster weatherForecaster)
    {
        _weatherForecaster = weatherForecaster;
    }
    
    public async Task BroadcastWeather()
    {
        for (int i = 1; i <= 10; i++)
        {
            var forecast = await _weatherForecaster.GetWeatherForecastAsync(i);
            var json = System.Text.Json.JsonSerializer.Serialize(forecast);
            await Clients.All.SendAsync("ReceiveWeather", json);    
        }
    }
}