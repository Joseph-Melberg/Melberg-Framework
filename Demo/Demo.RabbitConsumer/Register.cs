using Demo.RabbitConsumer.Messages;
using Demo.RabbitConsumer.Service;
using MelbergFramework.Infrastructure.Rabbit;
using MelbergFramework.Infrastructure.Rabbit.Translator;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.RabbitConsumer;

public class Register
{
    public static IServiceCollection RegisterServices(IServiceCollection services)
    {
        RabbitModule.RegisterConsumer<DemoRabbitConsumer>(services);
        services.AddTransient<IJsonToObjectTranslator<TestMessage>,JsonToObjectTranslator<TestMessage>>();
        return services;
    }
}