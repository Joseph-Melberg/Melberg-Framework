using MelbergFramework.Application;

namespace Demo.RabbitConsumerWithMetric;
class Program
{
    static async Task Main(string[] args)
    {
        await MelbergHost.CreateDefaultApp<Startup>().Build().Begin(CancellationToken.None);
    }   
}