using Melberg.Application;
using Melberg.Application.Health;
using Melberg.Core.Health;
using Melberg.Infrastructure.Rabbit;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.RabbitPublisher;

public class Startup : IAppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        RabbitModule.RegisterPublisher<TestMessage>(services);
        RabbitModule.RegisterPublisher<TestMessage2>(services);
    }
}