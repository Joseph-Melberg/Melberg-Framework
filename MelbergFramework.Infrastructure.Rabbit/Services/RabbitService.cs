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
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MelbergFramework.Infrastructure.Rabbit;

public class RabbitService : BackgroundService
{

    private readonly IStandardConsumer _consumer;
    private readonly ILogger _logger;
    private readonly IStandardConnectionFactory _connectionFactory;
    private readonly IRabbitConfigurationProvider _configurationProvider;
    private readonly RabbitConsumerConfiguration _consumerConfiguration;

    public override Task ExecuteTask => base.ExecuteTask;

    public RabbitService(
        IStandardConsumer consumer,
        IRabbitConfigurationProvider configurationProvider, 
        IStandardConnectionFactory connectionFactory, 
        IOptions<RabbitConsumerConfiguration> consumerConfiguration,
        ILogger logger)
    {
        _consumer = consumer;    
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
        await Task.Yield();
        _logger.LogInformation("Beginning Rabbit");
        
        var receiverConfig = _configurationProvider.GetAsyncReceiverConfiguration("AsyncRecievers");

        var connectionConfig = _configurationProvider.GetConnectionConfigData(receiverConfig.Connection);
        var channel = _connectionFactory.GetConsumerModel();

        var amqpObjects = _configurationProvider.GetAmqpObjectsConfiguration();


        channel.ConfigureExchanges(connectionConfig.Name,amqpObjects.ExchangeList, _logger);
        channel.ConfigureQueues(connectionConfig.Name,amqpObjects.QueueList, _logger);
        channel.ConfigureBindings(connectionConfig.Name,amqpObjects.BindingList, _logger);
        //foreach ...


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


            await ConsumeMessageAsync(message, cancellationToken);

            channel.BasicAck(ea.DeliveryTag, false);
            await Task.Yield();
        };
        for(int i = 0; i < _consumerConfiguration.Scale; i ++)
        {
            var consumerTag = channel.BasicConsume(receiverConfig.Queue, false, consumer);
        }

        await Task.Delay(Timeout.Infinite, cancellationToken); 
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

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new System.NotImplementedException();
    }
}