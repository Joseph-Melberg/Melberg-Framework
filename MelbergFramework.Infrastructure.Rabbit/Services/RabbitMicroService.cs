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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MelbergFramework.Infrastructure.Rabbit;
public class RabbitMicroService<TConsumer> : BackgroundService
where TConsumer : class, IStandardConsumer
{
    private readonly string _selector;
    private readonly IStandardConsumer _consumer;
    private readonly IRabbitConfigurationProvider _configurationProvider;
    private readonly IStandardConnectionFactory _connectionFactory;
    private readonly ILogger _logger;

    
    public RabbitMicroService(
        string selector,
        TConsumer consumer,
        IRabbitConfigurationProvider configurationProvider, 
        IStandardConnectionFactory connectionFactory, 
        ILogger logger)
    {
        _selector = selector;
        _consumer = consumer;
        _connectionFactory = connectionFactory;
        _configurationProvider = configurationProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var receiverConfig = _configurationProvider.GetAsyncReceiverConfiguration(_selector);

        var connectionConfig = _configurationProvider.GetConnectionConfigData(receiverConfig.Connection);

        var channel = _connectionFactory.GetConsumerModel(_selector);

        var amqpObjects = _configurationProvider.GetAmqpObjectsConfiguration();

        channel.ConfigureExchanges(connectionConfig.Name,amqpObjects.ExchangeList, _logger);
        channel.ConfigureQueues(connectionConfig.Name,amqpObjects.QueueList, _logger);
        channel.ConfigureBindings(connectionConfig.Name,amqpObjects.BindingList, _logger);
        
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (ch, ea) =>
        {
        
            var message = new Message()
            {
                RoutingKey = ea.RoutingKey,
                Headers = ea.BasicProperties.Headers ?? new Dictionary<string,object>(),
                Body = ea.Body.ToArray()
            };

            message.Timestamp = message.GetTimestamp();


            await ConsumeMessageAsync(message, stoppingToken);

            channel.BasicAck(ea.DeliveryTag, false);
            await Task.Yield();
        };

        var consumerTag = channel.BasicConsume(receiverConfig.Queue, false, consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken); 
    }

    public virtual async Task ConsumeMessageAsync(Message message, CancellationToken cancellationToken) 
    {
        try
        {
            await _consumer.ConsumeMessageAsync(message, cancellationToken);     
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    } 

}