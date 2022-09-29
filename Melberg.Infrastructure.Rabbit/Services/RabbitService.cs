using System.Threading;
using System.Threading.Tasks;
using Melberg.Core.Rabbit.Configurations;
using Melberg.Infrastructure.Rabbit.Configuration;
using Melberg.Infrastructure.Rabbit.Consumers;
using Melberg.Infrastructure.Rabbit.Messages;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Melberg.Infrastructure.Rabbit;

public class RabbitService : BackgroundService
{

    private readonly IStandardConsumer _consumer;
    private readonly IRabbitConfigurationProvider _configurationProvider;

    public override Task ExecuteTask => base.ExecuteTask;

    public RabbitService(IStandardConsumer consumer, IRabbitConfigurationProvider configurationProvider)
    {
        _consumer = consumer;    
        _configurationProvider = configurationProvider;
    }

    public void Dispose()
    {
        this.Dispose();
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        var receiverConfig = _configurationProvider.GetAsyncReceiverConfiguration("AsyncRecievers");

        var connectionConfig = _configurationProvider.GetConnectionConfigData(receiverConfig.Connection);
        var connectionFactory = new StandardConnectionFactory(connectionConfig);
        IConnection connection = connectionFactory.GetConnection();

        var channel = connection.CreateModel();

        var amqpObjects = _configurationProvider.GetAmqpObjectsConfiguration();

        channel.ConfigureExchanges(connectionConfig.Name,amqpObjects.ExchangeList);
        channel.ConfigureQueues(connectionConfig.Name,amqpObjects.QueueList);
        channel.ConfigureBindings(connectionConfig.Name,amqpObjects.BindingList);
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
        return Task.Delay(Timeout.Infinite, cancellationToken); 
    }

    public Task ConsumeMessageAsync(Message message, CancellationToken cancellationToken) => _consumer.ConsumeMessageAsync(message, cancellationToken);

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new System.NotImplementedException();
    }
}