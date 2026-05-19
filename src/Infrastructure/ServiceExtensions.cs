namespace Rheum.Infrastructure;

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Diagnostics;
using Rebus.Handlers;
using Rebus.Kafka;
using Rebus.Routing.TypeBased;
using Rheum.Shared;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Kafka");
        var queueName = "rheum-queue";

        services.AddTransient<IHandleMessages<Pong>, PongHandler>();

        services.AddRebus(configure => configure
            .Transport(transport => transport.UseKafka(connectionString, queueName))
            .Options(options => options.EnableDiagnosticSources())
            .Routing(route => route.TypeBased().Map<Ping>("ping-service-topic"))
        );

        services.AutoRegisterHandlersFromAssemblyOf<PongHandler>();

        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        services.AddHostedService<RheumWorker>();
    }
}
