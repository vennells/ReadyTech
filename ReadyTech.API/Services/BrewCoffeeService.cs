using ReadyTech.API.DTOs;
using ReadyTech.API.Interfaces;

namespace ReadyTech.API.Services;

public sealed class BrewCoffeeService(IDateTimeProvider dateTimeProvider)
{
    private static int _count;
    private static bool isFifthCall => _count % 5 == 0;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public BrewCoffee Brew()
    {
        if(_dateTimeProvider.Now.Month == 4 && _dateTimeProvider.Now.Day == 1)
        {
            return new BrewCoffee
            {
                StatusCode = 418,
            };
        }

        _count++;

        if (isFifthCall)
        {
            return new BrewCoffee
            {
                StatusCode = 503,
            };
        }

        return new BrewCoffee{
            StatusCode = 200,
            Message = "Your piping hot coffee is ready",
            Prepared = _dateTimeProvider.Now.ToString("o")
        };
    }
}
