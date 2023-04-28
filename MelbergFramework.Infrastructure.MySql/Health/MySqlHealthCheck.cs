using System;
using System.Threading;
using System.Threading.Tasks;
using MelbergFramework.Core.Health;
using Microsoft.Extensions.DependencyInjection;

namespace MelbergFramework.Infrastructure.MySql.Health;

public class MySqlHealthCheck<TContext> : HealthCheck
    where TContext : DefaultContext
{
    private readonly TContext _context;
    public MySqlHealthCheck(IServiceProvider _serviceProvider)
    {
        _context = _serviceProvider.GetService<TContext>(); 
    }

    public override string Name => typeof(TContext).Name+"_influxdb";
    public override Task<bool> IsOk(CancellationToken token)
    {
        return Task.FromResult(true);
    }
}