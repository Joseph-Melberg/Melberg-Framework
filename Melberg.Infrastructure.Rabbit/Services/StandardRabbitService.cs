using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Melberg.Core.Rabbit.Configurations;
using Melberg.Infrastructure.Rabbit.Configuration;
using Melberg.Infrastructure.Rabbit.Consumers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Melberg.Infrastructure.Rabbit.Services;

public class StandardRabbitService : IStandardRabbitService
{
    private readonly IStandardConsumer _consumer;
    private readonly IRabbitConfigurationProvider _configurationProvider;
    public StandardRabbitService(IStandardConsumer consumer, IRabbitConfigurationProvider configurationProvider)
    {
        _consumer = consumer;    
        _configurationProvider = configurationProvider;
    }
    public async Task Run()
    {
        var receiverConfig = _configurationProvider.GetAsyncReceiverConfiguration("AsyncRecievers");

        var connectionConfig = _configurationProvider.GetConnectionConfigData(receiverConfig.Connection);
        ConnectionFactory factory = new ConnectionFactory();
        factory.UserName = connectionConfig.UserName;
        factory.Password = connectionConfig.Password;
        factory.VirtualHost = "/";
        factory.DispatchConsumersAsync = true;
        factory.HostName = connectionConfig.ServerName;
        factory.ClientProvidedName = connectionConfig.ClientName;

        IConnection connection = factory.CreateConnection();

        var channel = connection.CreateModel();

        var amqpObjects = _configurationProvider.GetAmqpObjectsConfiguration();

        channel.ConfigureExchanges(connectionConfig.Name,amqpObjects.ExchangeList);
        channel.ConfigureQueues(connectionConfig.Name,amqpObjects.QueueList);
        channel.ConfigureBindings(connectionConfig.Name,amqpObjects.BindingList);
        //foreach ...
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += async (ch, ea) =>
        {
        
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            await ConsumeMessageAsync(message);

            channel.BasicAck(ea.DeliveryTag, false);
            await Task.Yield();

        };
        var consumerTag = channel.BasicConsume(receiverConfig.Queue, false, consumer);
        await Task.Delay(Timeout.Infinite); 
    }

    public Task ConsumeMessageAsync(string message) => _consumer.ConsumeMessageAsync(message);
}