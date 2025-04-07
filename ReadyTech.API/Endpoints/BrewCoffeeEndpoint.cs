using ReadyTech.API.Services;
using ReadyTech.API.Interfaces;

namespace ReadyTech.API.Endpoints;

public static class BrewCoffeeEndpoint
{
    public static RouteHandlerBuilder MapBrewCoffeeEndpoint(this IEndpointRouteBuilder routes)
    {
        return routes.MapGet("/brew-coffee", (BrewCoffeeService coffeeService, IWeatherService weatherService) =>
        {
            var result = coffeeService.Brew();
            var test = weatherService.GetCurrentTemp();
            Console.WriteLine(test.Status.ToString());
            Console.WriteLine(test.Result.ToString());
            //if (test.IsCompletedSuccessfully)
            //{
            //    var weather = test.Result;
            //    result.Weather = weather.ToString();
            //}

            if (result.StatusCode != 200)
            {
                return Results.StatusCode(result.StatusCode);
            }

            return TypedResults.Json(result);
        })
        .WithName("GetBrewCoffee");
    }
}
