using Demo.MySql.Infrastructure;
using Melberg.Infrastructure.MySql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.MySql;



class Program
{

    public static IConfigurationRoot configuration;
    private static IServiceProvider _serviceProvider;
    static async Task Main(string[] args)
    {
        RegisterServices();
        var repo =  _serviceProvider.GetRequiredService<ITestRepository>();
        var j = repo.Get();
        repo.Test();
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
        MySqlModule.LoadSqlRepository<ITestRepository,TestRepository,ReadWriteContext>(services);
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