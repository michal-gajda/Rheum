namespace Rheum.WebApi;

using OpenTelemetry;
using OpenTelemetry.Logs;

internal sealed class HealthCheckLogFilter : BaseProcessor<LogRecord>
{
    public override void OnEnd(LogRecord data)
    {
        if (!HealthRequestContext.IsHealthRequest)
            base.OnEnd(data);
    }
}
