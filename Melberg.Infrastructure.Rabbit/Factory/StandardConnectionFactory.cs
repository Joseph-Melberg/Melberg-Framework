using System.Collections.Generic;
using Melberg.Core.Rabbit.Configurations;
using Melberg.Core.Rabbit.Configurations.Data;
using RabbitMQ.Client;

namespace Melberg.Infrastructure.Rabbit.Factory;

public class StandardConnectionFactory : IStandardConnectionFactory
{
    private readonly IRabbitConfigurationProvider _configurationProvider;
    private IConnection _consumerConnection;

    private Dictionary<string,IConnection> _publisherConnections = new Dictionary<string, IConnection>();
    public StandardConnectionFactory(IRabbitConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;
    }
    private IConnection GenerateConsumerChannel()
    {
        var receiverConfig = _configurationProvider.GetAsyncReceiverConfiguration("AsyncRecievers");
        var connectionConfig = _configurationProvider.GetConnectionConfigData(receiverConfig.Connection);
        return MakeNewConnection(connectionConfig);
    }

    private IConnection GeneratePublisherChannel(string name)
    {
        var _config = _configurationProvider.GetPublisherConfiguration(name);
        var connectionConfig = _configurationProvider.GetConnectionConfigData(_config.Connection);
        return MakeNewConnection(connectionConfig);
    }

    public IConnection GetConsumerChannel()
    {
        if(_consumerConnection == null)    
        {
            _consumerConnection = GenerateConsumerChannel();
        }
        return _consumerConnection;
    }

    private IConnection MakeNewConnection(ConnectionFactoryConfigData connectionConfig)
    {
        var factory = new ConnectionFactory();
        factory.UserName = connectionConfig.UserName;
        factory.Password = connectionConfig.Password;
        factory.VirtualHost = "/";
        factory.DispatchConsumersAsync = true;
        factory.HostName = connectionConfig.ServerName;
        factory.ClientProvidedName = connectionConfig.ClientName;
        return factory.CreateConnection();
    }

    public IConnection GetPublisherChannel(string name)
    {
        if(!_publisherConnections.ContainsKey(name))
        {
            _publisherConnections.Add(name,GeneratePublisherChannel(name));
        }
        return _publisherConnections[name];
    }
}