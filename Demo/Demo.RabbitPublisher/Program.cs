using MelbergFramework.Application;
using MelbergFramework.Infrastructure.Rabbit;
using MelbergFramework.Infrastructure.Rabbit.Publishers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.RabbitPublisher;

class Program
{
    public static IConfigurationRoot configuration;
    private static IServiceProvider _serviceProvider;

    public static async Task Main()
    {
        var host = MelbergHost.CreateDefaultApp<Startup>().Build();
        // var publisher = host.Services.GetRequiredService<IStandardPublisher<TestMessage>>();
        // publisher.Send(new TestMessage(){Value = "Howdy"});
        await host.Begin(CancellationToken.None);
    }

    private static void DisposeServices()
    {
        if (_serviceProvider == null)
        {
            return;
        }
        if (_serviceProvider is IDisposable)
        {
            ((IDisposable)_serviceProvider).Dispose();
        }
    }
}
