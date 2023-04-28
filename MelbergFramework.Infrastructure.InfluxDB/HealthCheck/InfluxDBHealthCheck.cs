
using System;
using System.Threading;
using System.Threading.Tasks;
using InfluxDB.Client;
using MelbergFramework.Core.Health;
using Microsoft.Extensions.DependencyInjection;

namespace MelbergFramework.Infrastructure.InfluxDB.Health;

public class InfluxDBHealthCheck<TContext> : HealthCheck
    where TContext : DefaultContext    
{
    private readonly InfluxDBClient _client;
    public InfluxDBHealthCheck(IServiceProvider serviceProvider)
    {
        _client = serviceProvider.GetService<IStandardInfluxDBClientFactory>().GetClient(typeof(TContext).Name);
    }

    public override string Name => typeof(TContext).Name+"_influxdb";

    public override Task<bool> IsOk(CancellationToken token)
    {
        return _client.PingAsync();
    }
}