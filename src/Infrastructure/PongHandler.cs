namespace Rheum.Infrastructure;

using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;
using Rheum.Domain.Telemetry;
using Rheum.Shared;

public sealed class PongHandler(ILogger<PongHandler> logger, TimeProvider timeProvider) : IHandleMessages<Pong>
{
    public async Task Handle(Pong message)
    {
        var roundTripSeconds = TimeSpan.FromTicks(timeProvider.GetUtcNow().Ticks - message.PingSentAtUtcTicks).TotalSeconds;

        using var activity = Activity.Current;
        activity?.SetTag("messaging.machine", Environment.MachineName);
        activity?.SetTag("pong.message", message.Message);
        activity?.SetTag("ping_pong.roundtrip_seconds", roundTripSeconds);

        logger.LogInformation("Received pong: {Message} on {Machine} (round-trip: {RoundTripSeconds:F3}s)", message.Message, Environment.MachineName, roundTripSeconds);

        DomainMetrics.PingPongRoundTripDuration.Record(roundTripSeconds);
    }
}
