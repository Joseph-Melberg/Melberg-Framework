using System;
using System.Linq;
using MelbergFramework.Core.Health;
using MelbergFramework.Core.Rabbit;
using MelbergFramework.Core.Rabbit.Configurations;
using MelbergFramework.Infrastructure.Rabbit.Configuration;
using MelbergFramework.Infrastructure.Rabbit.Consumers;
using MelbergFramework.Infrastructure.Rabbit.Factory;
using MelbergFramework.Infrastructure.Rabbit.Health;
using MelbergFramework.Infrastructure.Rabbit.Messages;
using MelbergFramework.Infrastructure.Rabbit.Metrics;
using MelbergFramework.Infrastructure.Rabbit.Publishers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MelbergFramework.Infrastructure.Rabbit
{
    public static class RabbitModule
    {
        public static void RegisterMicroConsumer<TConsumer>(IServiceCollection catalog)
        where TConsumer : class, IStandardConsumer
        {
            catalog.AddTransient<TConsumer,TConsumer>();
            catalog.AddHostedService(
                (s) => new RabbitMicroService<TConsumer>(
                    s.GetService<TConsumer>(),
                    s.GetService<IRabbitConfigurationProvider>(),
                    s.GetService<IStandardConnectionFactory>(),
                    s.GetService<ILogger>())
            );
        }
        
        public static void RegisterConsumer<TConsumer>(IServiceCollection catalog)
        where TConsumer : class, IStandardConsumer
        {
            RegisterConsumerRequireds<TConsumer>(catalog);
            catalog.AddHostedService<RabbitService>();
        }

        public static void RegisterConsumerWithMetrics<TConsumer>(IServiceCollection catalog)
        where TConsumer : class, IStandardConsumer
        {
            RegisterConsumerRequireds<TConsumer>(catalog);
            catalog.AddHostedService<RabbitServiceWithMetrics>();
            RegisterMetrics(catalog);
        }

        private static void RegisterConsumerRequireds<TConsumer>(IServiceCollection catalog)
        where TConsumer : class, IStandardConsumer
        {
            catalog.AddTransient<IStandardConsumer,TConsumer>();
            catalog.AddOptions<RabbitConsumerConfiguration>()
                .BindConfiguration("Rabbit");
            catalog.AddSingleton<IHealthCheck,RabbitConsumerHealthCheck>();
            catalog.AddSingleton<IStandardConnectionFactory, StandardConnectionFactory>();
            catalog.AddSingleton<IRabbitConfigurationProvider,RabbitConfigurationProvider>();
        }

        public static void RegisterMetrics(IServiceCollection catalog)
        {
            catalog.AddTransient<IMetricPublisher, MetricPublisher>();
            RegisterPublisher<MetricMessage>(catalog);
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