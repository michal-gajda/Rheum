namespace Rheum.Application.QueryResults;

public sealed record class WeatherForecastResult
{
    public required DateTime Date { get; init; }
    public required int TemperatureC { get; init; }
    public required int TemperatureF { get; init; }
    public string? Summary { get; init; }
}
