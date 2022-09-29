using Microsoft.Extensions.DependencyInjection;

namespace Melberg.Application;

public static class ApplicationModule
{
    public static void AddKeepAlive(IServiceCollection services)
    {
        services.AddHostedService<KeepAliveService>();
    }
}