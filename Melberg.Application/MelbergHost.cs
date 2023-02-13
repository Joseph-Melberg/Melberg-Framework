using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Melberg.Application.Health;
using Melberg.Core.Health;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Melberg.Application;

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
                .AddJsonFile("appsettings.json", false)
                .Build())
            .ConfigureServices((hbc, s) => 
        {
            
            var sup = ActivatorUtilities.CreateInstance<Service>(ServiceProvider);
            s.AddLogging( (a) => a.SetMinimumLevel(LogLevel.Information) .AddConsole()).BuildServiceProvider();

            var logger = ServiceProvider.GetService<ILoggerFactory>().CreateLogger<Service>();
            s.AddSingleton<ILogger>(logger);
            sup.ConfigureServices(s);

            s.AddHostedService<HealthCheckBackgroundService>();
            s.AddSingleton<IHealthCheckChecker,HealthCheckChecker>();
            s.AddSingleton(sup);
        }
        );

    }

    public static async Task Begin(this IHost host, CancellationToken token) 
    {
        var tasks  = host.Services.GetServices<IHostedService>();
        var tasksToRun = tasks.Select(_ => Task.Run(()=> _.StartAsync(token)));
        await Task.WhenAll(Task.WhenAll(tasksToRun),  Task.Delay(Timeout.Infinite));
    }
}