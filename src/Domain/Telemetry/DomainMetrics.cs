namespace Rheum.Domain.Telemetry;

using System.Diagnostics.Metrics;

public static class DomainMetrics
{
    public const string METER_NAME = "Rheum.Service";
    private static readonly Meter Meter = new(METER_NAME, "1.0.0");

    public static readonly Histogram<double> ApplicationCommandDuration = Meter.CreateHistogram<double>(name: "rheum.application.command.duration", unit: "s", description: "The processing time for business commands in the application layer.");

    public static readonly Histogram<double> PingPongRoundTripDuration = Meter.CreateHistogram<double>(name: "rheum.ping_pong.roundtrip.duration", unit: "s", description: "The total round-trip time from sending Ping to receiving Pong.");
}
