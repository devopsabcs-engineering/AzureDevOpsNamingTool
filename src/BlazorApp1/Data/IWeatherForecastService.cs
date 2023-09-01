namespace BlazorApp1.Data
{
    public interface IWeatherForecastService
    {
        Task<WeatherForecast[]> GetForecastAsync(DateOnly startDate);
    }
}