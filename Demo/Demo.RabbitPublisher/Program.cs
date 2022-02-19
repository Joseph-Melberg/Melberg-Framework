using Melberg.Infrastructure.Rabbit;
using Melberg.Infrastructure.Rabbit.Publishers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.RabbitPublisher;

class Program
{
    public static IConfigurationRoot configuration;
    private static IServiceProvider _serviceProvider;

    public static void Main()
    {
        var services = new ServiceCollection();
        configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json", false)
            .Build();

        services.AddSingleton<IConfiguration>(configuration);
        RabbitModule.RegisterPublisher<TestMessage>(services);
        _serviceProvider = services.BuildServiceProvider();
        var publisher = _serviceProvider.GetRequiredService<IStandardPublisher<TestMessage>>();
        publisher.Send(new TestMessage(){Value = "Howdy"});
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
