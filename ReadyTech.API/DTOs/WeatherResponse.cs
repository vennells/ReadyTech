namespace ReadyTech.API.DTOs;

public class WeatherResponse
{
    public CurrentWeather Current { get; set; }
}

public class CurrentWeather
{
    public int Temp { get; set; }
}
