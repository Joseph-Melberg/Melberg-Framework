using System.Threading;
using System.Threading.Tasks;
using Demo.MySql.Infrastructure;
using Melberg.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.MySql;



    class Program
    {
        static async Task Main(string[] args)
        {
            var host = MelbergHost.CreateDefaultApp<Startup>().Build();
            host.Services.GetService<ITestRepository>().Test();
            await host.Begin(CancellationToken.None);
            //var tasks = servies.Select(_ => Task.Run(()=> _.StartAsync(CancellationToken.None)));
            //await Task.WhenAll(tasks);

        }   
    }