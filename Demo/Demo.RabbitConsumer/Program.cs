using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Melberg.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.RabbitConsumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await MelbergHost.CreateDefaultApp<Startup>().Build().Begin(CancellationToken.None);
            //var tasks = servies.Select(_ => Task.Run(()=> _.StartAsync(CancellationToken.None)));
            //await Task.WhenAll(tasks);

        }   
    }
}
