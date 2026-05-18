namespace Rheum.WebApi;

using AspNetCore.SignalR.OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Rebus.OpenTelemetry.Configuration;
using Rheum.Application;
using Rheum.Infrastructure;
using Rheum.Application.Common.Shared;

public sealed class Program
{
    private Program()
    {
    }

    public static async Task<int> Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string serviceName = ServiceConstants.ServiceName;
        const string serviceNamespace = "Rheum";
        string serviceVersion = ServiceConstants.ServiceVersion;
        const string serviceInstanceId = "instance-1";

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName, serviceNamespace, serviceVersion, autoGenerateServiceInstanceId: false, serviceInstanceId: serviceInstanceId);

        builder.Logging.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(resourceBuilder);
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
            options.ParseStateValues = true;
            options.AddOtlpExporter();
        });

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
                .SetResourceBuilder(resourceBuilder)
                .SetSampler(new AlwaysOnSampler())
                .AddAspNetCoreInstrumentation(options => options.RecordException = true)
                .AddHttpClientInstrumentation(options => options.RecordException = true)
                .AddRebusInstrumentation()
                .AddSignalRInstrumentation()
                .AddOtlpExporter())
            .WithMetrics(metrics => metrics
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddProcessInstrumentation()
                .AddRebusInstrumentation()
                .AddRuntimeInstrumentation()
                .AddOtlpExporter());

        builder.Services.AddHealthChecks();

        builder.Services.AddSignalR().AddHubInstrumentation();

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.UseHealthChecks("/healthz");

        app.MapOpenApi();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "Rheum API v1");
        });

        app.UseAuthorization();

        app.MapControllers();

        using var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger<Program>();

        try
        {
            await app.RunAsync();

            return Environment.ExitCode;
        }
        catch (Exception exception)
        {
            logger.LogCritical(exception, "{Message}", exception.Message);

            return -1;
        }
    }
}
