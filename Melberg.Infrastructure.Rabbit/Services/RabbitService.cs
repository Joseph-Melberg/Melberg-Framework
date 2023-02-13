using System.Threading;
using System.Threading.Tasks;
using Melberg.Core.Rabbit.Configurations;
using Melberg.Infrastructure.Rabbit.Configuration;
using Melberg.Infrastructure.Rabbit.Consumers;
using Melberg.Infrastructure.Rabbit.Factory;
using Melberg.Infrastructure.Rabbit.Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Melberg.Infrastructure.Rabbit;

public class RabbitService : BackgroundService
{

    private readonly IStandardConsumer _consumer;
    private readonly ILogger _logger;
    private readonly IStandardConnectionFactory _connectionFactory;
    private readonly IRabbitConfigurationProvider _configurationProvider;

    public override Task ExecuteTask => base.ExecuteTask;

    public RabbitService(
        IStandardConsumer consumer,
        IRabbitConfigurationProvider configurationProvider, 
        IStandardConnectionFactory connectionFactory, 
        ILogger logger)
    {
        _consumer = consumer;    
        _logger = logger;
        _configurationProvider = configurationProvider;
        _connectionFactory = connectionFactory;
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
        IConnection connection = _connectionFactory.GetConsumerChannel();
        var channel = connection.CreateModel();
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
                Headers = ea.BasicProperties.Headers,
                Body = ea.Body.ToArray()
            };

            await ConsumeMessageAsync(message, cancellationToken);

            channel.BasicAck(ea.DeliveryTag, false);
            await Task.Yield();
        };

        var consumerTag = channel.BasicConsume(receiverConfig.Queue, false, consumer);
        await Task.Delay(Timeout.Infinite, cancellationToken); 
    }

    public Task ConsumeMessageAsync(Message message, CancellationToken cancellationToken) => _consumer.ConsumeMessageAsync(message, cancellationToken);

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new System.NotImplementedException();
    }
}