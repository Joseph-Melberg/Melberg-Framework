using MelbergFramework.Core.Health;
using MelbergFramework.Core.InfluxDB;
using MelbergFramework.Infrastructure.InfluxDB.Health;
using Microsoft.Extensions.DependencyInjection;

namespace MelbergFramework.Infrastructure.InfluxDB;

public class InfluxDBModule
{
    public static void LoadInfluxDBRepository<TFrom, TTo, TContext>(IServiceCollection catalog)
        where TTo : BaseInfluxDBRepository<TContext>,TFrom
        where TFrom : class
        where TContext : DefaultContext
        {
            catalog.AddSingleton<IInfluxDBConfigurationProvider, InfluxDBConfigurationProvider>();
        
            catalog.AddTransient<TFrom, TTo>();
        
            catalog.AddTransient<TContext, TContext>();

            catalog.AddSingleton<IStandardInfluxDBClientFactory, StandardInfluxDBClientFactory>();

            catalog.AddSingleton<IHealthCheck, InfluxDBHealthCheck<TContext>>();
        }
}