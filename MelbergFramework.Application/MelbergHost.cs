using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MelbergFramework.Application.Health;
using MelbergFramework.Core.Health;
using MelbergFramework.Core.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MelbergFramework.Application;

public static class MelbergHost
{
    public static IServiceProvider ServiceProvider;
    public static IHostBuilder CreateDefaultApp<Service>()  where Service : class, IAppStartup
    {


        return Host.CreateDefaultBuilder() 
            .ConfigureServices(
                (s) => ServiceProvider =
                 s.AddLogging(
                    (a) => a.SetMinimumLevel(LogLevel.Information)
                            .AddConsole()).BuildServiceProvider())
            .ConfigureAppConfiguration((conf) => conf
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", true, reloadOnChange: false)
                .Build())
            .ConfigureServices((hbc, s) => 
        {
            
            var sup = ActivatorUtilities.CreateInstance<Service>(ServiceProvider);
            s
            .AddLogging(
                (a) => a
                .SetMinimumLevel(LogLevel.Information)
                .AddConsole())
            .BuildServiceProvider();

            var logger = ServiceProvider.GetService<ILoggerFactory>().CreateLogger<Service>();
            s.AddSingleton<ILogger>(logger);
            sup.ConfigureServices(s);
            s.AddHostedService<HealthCheckBackgroundService>();
            s.AddSingleton<IHealthCheckChecker,HealthCheckChecker>();
            s.AddSingleton(sup);
            s.Configure<ApplicationConfiguration>(hbc.Configuration.GetSection(ApplicationConfiguration.Section));
        }
        );

    }

    public static async Task Begin(this IHost host, CancellationToken token) 
    {
        var tasks  = host.Services.GetServices<IHostedService>();
        var tasksToRun = tasks.Select(_ => Task.Run(()=> _.StartAsync(token)));
        try
        {
            
            await Task.WhenAll(Task.WhenAll(tasksToRun),  Task.Delay(Timeout.Infinite));
        }
        catch (System.Exception ex)
        {
            
            throw;
        }
    }
}