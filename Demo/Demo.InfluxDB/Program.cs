using Demo.InfluxDB.Infrastructure.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.InfluxDB;

class Program
{

    public static IConfigurationRoot configuration;
    private static IServiceProvider _serviceProvider;
    static async Task Main(string[] args)
    {
        RegisterServices();
        var repo =  _serviceProvider.GetRequiredService<ITestRepo>();
        await repo.Test();
        DisposeServices();
    }

    private static void RegisterServices()
    {
        var services = new ServiceCollection();
        configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json", false)
            .Build();

        services.AddSingleton<IConfiguration>(configuration);
        Register.RegisterServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }

    private static void DisposeServices()
    {
        if (_serviceProvider == null)
        {
            return;
        }
        if (_serviceProvider is IDisposable)
        {
            ((IDisposable)_serviceProvider).Dispose();
        }
    }
}