using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MelbergFramework.Core.Rabbit;
using MelbergFramework.Core.Rabbit.Configurations;
using MelbergFramework.Infrastructure.Rabbit.Configuration;
using MelbergFramework.Infrastructure.Rabbit.Consumers;
using MelbergFramework.Infrastructure.Rabbit.Extensions;
using MelbergFramework.Infrastructure.Rabbit.Factory;
using MelbergFramework.Infrastructure.Rabbit.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MelbergFramework.Infrastructure.Rabbit;

public class RabbitService : BackgroundService
{

    private readonly ILogger _logger;
    private readonly IStandardConnectionFactory _connectionFactory;
    private readonly IRabbitConfigurationProvider _configurationProvider;
    private readonly RabbitConsumerConfiguration _consumerConfiguration;
    private readonly IServiceProvider _provider;
    public override Task ExecuteTask => base.ExecuteTask;

    public RabbitService(
        IRabbitConfigurationProvider configurationProvider, 
        IStandardConnectionFactory connectionFactory, 
        IOptions<RabbitConsumerConfiguration> consumerConfiguration,
        IServiceProvider provider,
        ILogger logger)
    {
        _provider = provider;
        _logger = logger;
        _configurationProvider = configurationProvider;
        _connectionFactory = connectionFactory;
        _consumerConfiguration = consumerConfiguration.Value;
    }

    public void Dispose()
    {
        this.Dispose();
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Beginning Rabbit");
        
        ApplyConfiguration();

        var channel = _connectionFactory.GetConsumerModel();
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (ch, ea) => HandleMessage(ch, ea);

        for(int i = 0; i < _consumerConfiguration.Scale; i ++)
        {
            var consumerTag = channel.BasicConsume(receiverConfig.Queue, false, consumer);
        }

        await Task.Delay(Timeout.Infinite, cancellationToken); 
    }

    public virtual async Task HandleMessage(object ch, BasicDeliverEventArgs ea)
    {
        var scope = _provider.CreateAsyncScope();
        var consumerService = scope.ServiceProvider.GetService<IStandardConsumer>();
        var message = new Message()
        {
            RoutingKey = ea.RoutingKey,
            Headers = ea.BasicProperties.Headers ?? new Dictionary<string,object>(),
            Body = ea.Body.ToArray()
        };

        message.Timestamp = message.GetTimestamp();

        try
        {
            await consumerService.ConsumeMessageAsync(message,cancellationToken.None);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        channel.BasicAck(ea.DeliveryTag, false);

        await scope.DisposeAsync();
    }

    private void ApplyConfiguration()
    {
        var receiverConfig = _configurationProvider.GetAsyncReceiverConfiguration("AsyncRecievers");
        var connectionConfig = _configurationProvider.GetConnectionConfigData(receiverConfig.Connection);
        var amqpObjects = _configurationProvider.GetAmqpObjectsConfiguration();

        channel.ConfigureExchanges(connectionConfig.Name,amqpObjects.ExchangeList, _logger);
        channel.ConfigureQueues(connectionConfig.Name,amqpObjects.QueueList, _logger);
        channel.ConfigureBindings(connectionConfig.Name,amqpObjects.BindingList, _logger);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new System.NotImplementedException();
    }
}
