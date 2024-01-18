using System.Collections.Concurrent;
using MelbergFramework.Core.Rabbit.Configurations;
using MelbergFramework.Core.Rabbit.Configurations.Data;
using RabbitMQ.Client;

namespace MelbergFramework.Infrastructure.Rabbit.Factory;

public class StandardConnectionFactory : IStandardConnectionFactory
{
    private readonly IRabbitConfigurationProvider _configurationProvider;
    private static IModel _consumerChannel;

    private static ConcurrentDictionary<string,IConnection> _publisherConnections = new ConcurrentDictionary<string, IConnection>();
    public StandardConnectionFactory( IRabbitConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;
    }
    private IConnection GenerateConsumerConnection(string consumerName)
    {
        var receiverConfig = _configurationProvider.GetAsyncReceiverConfiguration(consumerName);
        var connectionConfig = _configurationProvider.GetConnectionConfigData(receiverConfig.Connection);
        return MakeNewConnection(connectionConfig);
    }

    private IConnection GeneratePublisherChannel(string name)
    {
        var _config = _configurationProvider.GetPublisherConfiguration(name);
        var connectionConfig = _configurationProvider.GetConnectionConfigData(_config.Connection);
        return MakeNewConnection(connectionConfig);
    }

    public IModel GetConsumerModel(string name = "IncommingMessages")
    {
        _consumerChannel??= GenerateConsumerConnection(name).CreateModel();
        
        return _consumerChannel;
    } 
    private IConnection MakeNewConnection(ConnectionFactoryConfigData connectionConfig)
    {
        var factory = new ConnectionFactory()
        {
            UserName = connectionConfig.UserName,
            Password = connectionConfig.Password,
            VirtualHost = "/",
            DispatchConsumersAsync = true,
            HostName = connectionConfig.ServerName,
            ClientProvidedName = connectionConfig.ClientName,
        };
        return factory.CreateConnection();
    }

    public IConnection GetPublisherChannel(string name)
    {
        return _publisherConnections.GetOrAdd(name, GeneratePublisherChannel(name));
    }
}