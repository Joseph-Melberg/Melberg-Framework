using Demo.RabbitConsumer.Service;
using Melberg.Application;
using Melberg.Infrastructure.Rabbit;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.RabbitConsumer;

public class Startup : IAppStartup
{
    public  void ConfigureServices(IServiceCollection services)
    {
        RabbitModule.RegisterConsumer<DemoRabbitConsumer>(services);
        ApplicationModule.AddKeepAlive(services);
        Register.RegisterServices(services);
    }
}