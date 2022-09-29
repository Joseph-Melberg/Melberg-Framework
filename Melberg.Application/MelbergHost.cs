using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Melberg.Application;

public static class MelbergHost
{
    public static IServiceProvider ServiceProvider;
    public static IHostBuilder CreateDefaultApp<Service>()  where Service : class, IAppStartup
    {


        return Host.CreateDefaultBuilder() 
            .ConfigureServices((s) => ServiceProvider = s.BuildServiceProvider())
            .ConfigureAppConfiguration((conf) => conf
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build())
            .ConfigureServices((hbc, s) => 
        {
            var sup = ActivatorUtilities.CreateInstance<Service>(ServiceProvider);

            sup.ConfigureServices(s);

            s.AddSingleton(sup);
        }
        );

    }
}