using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ReadyTech.API.Interfaces;
using ReadyTech.API.Mocks;
using Moq;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ReadyTech.API.IntegrationTests;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    //public Mock<IDateTimeProvider> DateTimeProvider { get; } = new Mock<IDateTimeProvider>();
    public MockDateTimeProvider DateTimeProvider { get; } = new();
    public Mock<IWeatherService> WeatherService { get; } = new Mock<IWeatherService>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
               d => d.ServiceType == typeof(IDateTimeProvider));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.RemoveAll(typeof(IWeatherService));


            services.AddSingleton<IDateTimeProvider>(DateTimeProvider);
            services.AddSingleton(WeatherService.Object);
        });
    }
}