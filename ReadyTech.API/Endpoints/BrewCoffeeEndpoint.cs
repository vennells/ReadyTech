using ReadyTech.API.Services;

namespace ReadyTech.API.Endpoints
{
    public static class BrewCoffeeEndpoint
    {
        public static RouteHandlerBuilder MapBrewCoffeeEndpoint(this IEndpointRouteBuilder routes)
        {
            return routes.MapGet("/brew-coffee", (BrewCoffeeService coffeeService) =>
            {
                var result = coffeeService.Brew();

                if (result.StatusCode != 200)
                {
                    return Results.StatusCode(result.StatusCode);
                }

                return TypedResults.Json(result);
            })
            .WithName("GetBrewCoffee");
        }
    }
}
