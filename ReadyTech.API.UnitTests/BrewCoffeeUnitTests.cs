using Moq;
using Moq.Protected;
using System.Net.Http.Json;
using ReadyTech.API.Services;
using FluentAssertions;
using Castle.Core.Configuration;
using Microsoft.Extensions.Options;
using ReadyTech.API.Types;

namespace ReadyTech.API.UnitTests;
public class BrewCoffeeUnitTests
{
    private readonly IConfiguration _config;
    public HttpResponseMessage GetHttpResponseMessage()
    {
        return new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Content = JsonContent.Create(new
            {
                current = new
                {
                    temp = 20.0
                }
            })
        };
    }

    [Fact]
    public async Task GetCurrentWeatherShouldReturnCurrentTemperature()
    {
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(GetHttpResponseMessage());

        var httpClient = new HttpClient(httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://api.openweathermap.org/data/3.0/onecall")
        };

        var config = Options.Create(new WeatherConfig
        {
            ApiKey = "fake-api-key",
            BaseUrl = "https://api.openweathermap.org/data/3.0/onecall"
        });


        var weatherService = new WeatherService(httpClient, config);

        var result = await weatherService.GetCurrentTemp();

        result.Should().Be(20);
    }
}
