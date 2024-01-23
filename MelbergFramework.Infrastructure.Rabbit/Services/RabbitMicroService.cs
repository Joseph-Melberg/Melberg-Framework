using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MelbergFramework.Core.Application;
using MelbergFramework.Core.Rabbit.Configurations;
using MelbergFramework.Infrastructure.Rabbit.Configuration;
using MelbergFramework.Infrastructure.Rabbit.Consumers;
using MelbergFramework.Infrastructure.Rabbit.Extensions;
using MelbergFramework.Infrastructure.Rabbit.Factory;
using MelbergFramework.Infrastructure.Rabbit.Messages;
using MelbergFramework.Infrastructure.Rabbit.Metrics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MelbergFramework.Infrastructure.Rabbit;
public class RabbitMicroService<TConsumer> : BackgroundService
where TConsumer : class, IStandardConsumer
{
    private readonly string _selector;
    private readonly string _metricName;
    private readonly IServiceProvider _serviceProvider;
    private readonly IRabbitConfigurationProvider _configurationProvider;
    private readonly IStandardConnectionFactory _connectionFactory;
    private readonly IMetricPublisher _metricPublisher;
    private readonly ILogger<RabbitMicroService<TConsumer>> _logger;

    public RabbitMicroService(
        string selector,
        IServiceProvider serviceProvider,
        IRabbitConfigurationProvider configurationProvider,
        IStandardConnectionFactory connectionFactory,
        IMetricPublisher metricPublisher,
        IOptions<ApplicationConfiguration> applicationOptions,
        ILogger<RabbitMicroService<TConsumer>> logger)
    {
        _selector = selector;
        _metricName = string.Join("_", applicationOptions.Value.Name, _selector, "consumer");
        _serviceProvider = serviceProvider;
        _connectionFactory = connectionFactory;
        _configurationProvider = configurationProvider;
        _metricPublisher = metricPublisher;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var receiverConfig = _configurationProvider.GetAsyncReceiverConfiguration(_selector);

        var connectionConfig = _configurationProvider.GetConnectionConfigData(receiverConfig.Connection);

        var channel = _connectionFactory.GetConsumerModel(_selector);

        var amqpObjects = _configurationProvider.GetAmqpObjectsConfiguration();

        channel.ConfigureExchanges(connectionConfig.Name, amqpObjects.ExchangeList, _logger);
        channel.ConfigureQueues(connectionConfig.Name, amqpObjects.QueueList, _logger);
        channel.ConfigureBindings(connectionConfig.Name, amqpObjects.BindingList, _logger);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (ch, ea) =>
        {
            var message = new Message()
            {
                RoutingKey = ea.RoutingKey,
                Headers = ea.BasicProperties.Headers ?? new Dictionary<string, object>(),
                Body = ea.Body.ToArray()
            };

            Trace.CorrelationManager.ActivityId = message.GetCoID();

            await ConsumeMessageAsync(message, stoppingToken);

            channel.BasicAck(ea.DeliveryTag, false);
            await Task.Yield();
        };
        var consumerTag = channel.BasicConsume(receiverConfig.Queue, false, consumer);
        await Task.Delay(Timeout.Infinite, stoppingToken);
        

    }

    public virtual async Task ConsumeMessageAsync(Message message, CancellationToken cancellationToken)
    {
        var name = _selector + "_consumer";
        Trace.CorrelationManager.StartLogicalOperation(name);

        var now = DateTime.UtcNow;
        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
            
                await scope
                    .ServiceProvider
                    .GetService<TConsumer>()
                    .ConsumeMessageAsync(message, cancellationToken);

                stopwatch.Stop();
                if (_metricPublisher != null)
                {
                    _metricPublisher.SendMetric(_metricName, stopwatch.ElapsedMilliseconds, now);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        Trace.CorrelationManager.StopLogicalOperation();
    }

}
