using MelbergFramework.Application.Health;
using MelbergFramework.Core.Application;
using MelbergFramework.Core.Health;
using Microsoft.Extensions.DependencyInjection;

namespace MelbergFramework.Application;

public static class ApplicationModule
{
    public static IServiceCollection RegisterRequired(this IServiceCollection services)
    {
        services.AddOptions<ApplicationConfiguration>()
            .BindConfiguration(ApplicationConfiguration.Section)
            .ValidateDataAnnotations();
        services.RegisterHealthCheck(); 

        return services;

    }
    public static IServiceCollection RegisterHealthCheck(this IServiceCollection services) =>
        services
            .AddHostedService<HealthCheckBackgroundService>()
            .AddSingleton<IHealthCheckChecker,HealthCheckChecker>();
}