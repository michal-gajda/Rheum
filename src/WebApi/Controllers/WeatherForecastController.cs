namespace Rheum.WebApi.Controllers;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry;
using Rebus.Bus;
using Rheum.Application.Common.Shared;
using Rheum.Domain.Telemetry;
using Rheum.Shared;

[ApiController]
[Route("[controller]")]
public sealed class WeatherForecastController(IBus bus, ILogger<WeatherForecastController> logger, TimeProvider timeProvider) : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Balmy",
        "Bracing",
        "Chilly",
        "Cool",
        "Freezing",
        "Hot",
        "Mild",
        "Scorching",
        "Sweltering",
        "Warm",
    ];

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> GetAsync(CancellationToken cancellationToken = default)
    {
        long startTime = Stopwatch.GetTimestamp();
        string status = "success";

        using var activity = Activity.Current;
        activity?.SetTag("sender.machine", Environment.MachineName);

        // activity?.SetTag("rheum.correlation_id", $"{Guid.Empty}");
        // Baggage.SetBaggage("correlation_id", $"{Guid.Empty}");

        try
        {
            var dateTime = timeProvider.GetUtcNow().DateTime;

            var ping = new Ping
            {
                Message = $"{dateTime.Ticks}",
                SentAtUtcTicks = dateTime.Ticks
            };

            logger.LogInformation("Sending ping: {Message} on {Machine}", ping.Message, Environment.MachineName);
            await bus.Send(ping);

            // await Task.Delay(1500, cancellationToken);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(dateTime.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        catch (Exception)
        {
            status = "error";
            throw;
        }
        finally
        {
            DomainMetrics.ApplicationCommandDuration.Record(
                Stopwatch.GetElapsedTime(startTime).TotalSeconds,
                new KeyValuePair<string, object?>("command.name", "GetAsync"),
                new KeyValuePair<string, object?>("command.status", status)
            );
        }
    }
}
