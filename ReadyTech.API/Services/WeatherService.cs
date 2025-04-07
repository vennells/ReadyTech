using Microsoft.Extensions.Options;
using ReadyTech.API.DTOs;
using ReadyTech.API.Interfaces;
using ReadyTech.API.Types;
using System.Text.Json;

namespace ReadyTech.API.Services;

public class WeatherService(HttpClient httpClient, IOptions<WeatherConfig> options) : IWeatherService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly WeatherConfig _settings = options.Value;

    public async Task<int> GetCurrentTemp()
    {
        try
        {
            var lat = 40.7128;
            var lon = -74.0060;
            var part = "minutely,hourly,alerts";
            //var API_key = "5a6fb78f94e90823b99053b1779aeb55";
            var API_key = _settings.ApiKey;
            var partameters = $"?lat={lat}&lon={lon}&exclude={part}&appid={API_key}";
            var response = await _httpClient.GetAsync(partameters);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var weather = JsonSerializer.Deserialize<WeatherResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return weather?.Current?.Temp ?? 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 500;
        }
    }
}
