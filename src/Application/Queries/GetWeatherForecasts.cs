namespace Rheum.Application.Queries;

using MediatR;
using Rheum.Application.QueryResults;

public sealed record class GetWeatherForecasts : IRequest<IEnumerable<WeatherForecastResult>>
{
}
