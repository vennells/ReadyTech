using FluentAssertions;
using System.Net;
using ReadyTech.API.DTOs;
using System.Text.Json;
using Moq;

namespace ReadyTech.API.IntegrationTests
{
    public class BrewCoffeeIntegrationtTests: IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;


        public BrewCoffeeIntegrationtTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            // change the date if on april foolds day, otherwise tests will fail
            _factory.DateTimeProvider.Now = DateTime.Now.Month == 4 && DateTime.Now.Day == 1 ? new DateTime(2025, 5, 1) : DateTime.Now;
        }

        [Fact]
        public async Task GetBrewCoffeeReturnsOk()
        {
            var response = await _client.GetAsync("/brew-coffee");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetBrewCoffee_Under30_ReturnsExpectedJsonObject()
        {
            var expectedMessage = "Your piping hot coffee is ready";
            var expectedPreparedDate = DateTime.Now;
            _factory.WeatherService.Setup(ws => ws.GetCurrentTemp()).ReturnsAsync(20);

            var response = await _client.GetAsync("/brew-coffee");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<BrewCoffee>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            result.Should().NotBeNull();
            result.Message.Should().Be(expectedMessage);
            
            var actualPreparedDate = DateTime.Parse(result.Prepared);
            actualPreparedDate.Should().BeCloseTo(expectedPreparedDate, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task GetBrewCoffee_Over30_30ReturnsExpectedJsonObject()
        {
            var expectedMessage = "Your refreshing iced coffee is ready";
            var expectedPreparedDate = DateTime.Now;
            _factory.WeatherService.Setup(ws => ws.GetCurrentTemp()).ReturnsAsync(40);

            var response = await _client.GetAsync("/brew-coffee");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<BrewCoffee>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            result.Should().NotBeNull();
            result.Message.Should().Be(expectedMessage);

            var actualPreparedDate = DateTime.Parse(result.Prepared);
            actualPreparedDate.Should().BeCloseTo(expectedPreparedDate, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task GetBrewReturns503UnavailableOnEveryFifthCall()
        {
            for(int i = 1; i <= 10; i++)
            {
                var response = await _client.GetAsync("/brew-coffee");
                if (i % 5 == 0)
                {
                    response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
                }
                else
                {
                    response.StatusCode.Should().Be(HttpStatusCode.OK);
                }
            }
        }

        [Fact]
        public async Task GetBrewReturns418OnAprilFoolsDay()
        {
            _factory.DateTimeProvider.Now = new DateTime(2025, 4, 1);
            var response = await _client.GetAsync("/brew-coffee");
            response.StatusCode.Should().Be((HttpStatusCode)418);
        }
    }
}
