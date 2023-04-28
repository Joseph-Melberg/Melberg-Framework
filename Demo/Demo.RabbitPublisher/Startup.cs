using MelbergFramework.Application;
using MelbergFramework.Application.Health;
using MelbergFramework.Core.Health;
using MelbergFramework.Infrastructure.Rabbit;
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