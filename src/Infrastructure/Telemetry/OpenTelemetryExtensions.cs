namespace Rheum.Infrastructure.Telemetry;

using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Rheum.Domain.Telemetry;

public static class OpenTelemetryExtensions
{
    public static MeterProviderBuilder AddRheumInstrumentation(this MeterProviderBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder.AddMeter(DomainMetrics.METER_NAME);
    }

    public static TracerProviderBuilder AddRheumInstrumentation(this TracerProviderBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder.AddSource("Rheum.Service");
    }
}
