using Demo.InfluxDB.Infrastructure.Core;
using Melberg.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.InfluxDB;

class Program
{

    static async Task Main(string[] args)
    {
        var host = MelbergHost.CreateDefaultApp<Startup>().Build();
        
        var repo =  host.Services.GetService<ITestRepo>();
        await repo.Test();
        await host.Begin(CancellationToken.None);
    }
}