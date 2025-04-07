using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ReadyTech.API.Interfaces;
using ReadyTech.API.Mocks;

namespace ReadyTech.API.IntegrationTests;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public MockDateTimeProvider DateTimeProvider { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDateTimeProvider));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddSingleton<IDateTimeProvider>(DateTimeProvider);
        });
    }
}