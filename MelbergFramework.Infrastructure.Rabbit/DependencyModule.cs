using MelbergFramework.Core.Health;
using MelbergFramework.Core.Rabbit.Configurations;
using MelbergFramework.Infrastructure.Rabbit.Configuration;
using MelbergFramework.Infrastructure.Rabbit.Consumers;
using MelbergFramework.Infrastructure.Rabbit.Factory;
using MelbergFramework.Infrastructure.Rabbit.Health;
using MelbergFramework.Infrastructure.Rabbit.Messages;
using MelbergFramework.Infrastructure.Rabbit.Publishers;
using Microsoft.Extensions.DependencyInjection;

namespace MelbergFramework.Infrastructure.Rabbit
{
    public static class RabbitModule
    {
        public static void RegisterConsumer<TConsumer>(IServiceCollection catalog)
        where TConsumer : class, IStandardConsumer
        {
            catalog.AddSingleton<IStandardConnectionFactory, StandardConnectionFactory>();
            catalog.AddTransient<IStandardConsumer,TConsumer>();
            catalog.AddSingleton<IRabbitConfigurationProvider,RabbitConfigurationProvider>();
            catalog.AddHostedService<RabbitService>();
            catalog.AddSingleton<IHealthCheck,RabbitConsumerHealthCheck>();
        }

        public static void RegisterPublisher<TMessage>(IServiceCollection catalog)
            where TMessage : IStandardMessage
        {
            catalog.AddSingleton<IStandardConnectionFactory, StandardConnectionFactory>();
            catalog.AddTransient<IStandardPublisher<TMessage>,StandardPublisher<TMessage>>();
            catalog.AddSingleton<IRabbitConfigurationProvider,RabbitConfigurationProvider>();
            catalog.AddSingleton<IHealthCheck,RabbitPublisherHealthCheck<TMessage>>();
        }
    }

}