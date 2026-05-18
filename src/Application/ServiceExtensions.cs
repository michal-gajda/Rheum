namespace Rheum.Application;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
    }
}
