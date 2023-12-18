using MelbergFramework.Application.Health;
using MelbergFramework.Core.Health;
using Microsoft.Extensions.DependencyInjection;

namespace MelbergFramework.Application;

public static class ApplicationModule
{
    public static IServiceCollection RegisterHealthCheck(this IServiceCollection services) =>
        services
            .AddHostedService<HealthCheckBackgroundService>()
            .AddSingleton<IHealthCheckChecker,HealthCheckChecker>();
}