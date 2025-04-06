using ReadyTech.API.DTOs;
using ReadyTech.API.Interfaces;

namespace ReadyTech.API.Services;

public sealed class BrewCoffeeService
{
    private static int _count;
    private static bool isFifthCall => _count % 5 == 0;
    private readonly IDateTimeProvider _dateTimeProvider;

    public BrewCoffeeService(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public BrewCoffee Brew()
    {
        //if(DateTime.Now.Month == 4 && DateTime.Now.Day == 1)
        if(_dateTimeProvider.Now.Month == 4 && _dateTimeProvider.Now.Day == 1)
        {
            return new BrewCoffee
            {
                StatusCode = 418,
                //Message = "I'm a teapot",
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
