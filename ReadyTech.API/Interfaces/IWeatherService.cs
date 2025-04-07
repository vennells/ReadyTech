namespace ReadyTech.API.Interfaces;

public interface IWeatherService
{
    Task<int> GetCurrentTemp();
}
