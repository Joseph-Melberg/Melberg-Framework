using System.Threading.Tasks;
using Melberg.Application;

namespace Demo.RabbitConsumer
{
    class Program
    {
        static async Task Main(string[] args) => await MelbergHost.CreateDefaultApp<Startup>().Build().StartAsync();
    }
}
