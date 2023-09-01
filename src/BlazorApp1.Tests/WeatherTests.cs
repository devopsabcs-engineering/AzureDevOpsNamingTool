using BlazorApp1.Data;
using Bunit;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp1.Tests
{
    public class WeatherTests
    {
        //https://bunit.dev/docs/providing-input/inject-services-into-components.html
        [Fact]
        public void ShouldFetchData()
        {
            using var ctx = new TestContext();

            ctx.Services.AddSingleton<IWeatherForecastService>(new WeatherForecastService());

            // RenderComponent will inject the service in the WeatherForecasts component
            // when it is instantiated and rendered.
            var cut = ctx.RenderComponent<BlazorApp1.Pages.WeatherForecasts>();

            // Assert that service is injected
            Assert.NotNull(cut.Instance.Forecasts);
        }

    }
}
