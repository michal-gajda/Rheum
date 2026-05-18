namespace Rheum.Infrastructure;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal sealed class RheumWorker(IHostApplicationLifetime lifetime, ILogger<RheumWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        while (stoppingToken.IsCancellationRequested is false)
        {
            try
            {
                var i = 20;
                var j = 0;
                var k = i / j;
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                logger.LogError(exception, "{Message}", exception.Message);
                Environment.ExitCode = -1;
                lifetime.StopApplication();
                break;
            }
        }

        await Task.CompletedTask;
    }
}
