using System.Collections.Concurrent;
using System.Collections.Generic;
using MelbergFramework.Core.Rabbit.Configurations;
using MelbergFramework.Core.Rabbit.Configurations.Data;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace MelbergFramework.Infrastructure.Rabbit.Factory;

public class StandardConnectionFactory : IStandardConnectionFactory
{
    private readonly IRabbitConfigurationProvider _configurationProvider;
    private readonly ILogger _logger;
    private static IModel _consumerChannel;

    private static ConcurrentDictionary<string,IConnection> _publisherConnections = new ConcurrentDictionary<string, IConnection>();
    public StandardConnectionFactory(
        IRabbitConfigurationProvider configurationProvider,
        ILogger logger)
    {
        _logger = logger;
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
        if(_consumerChannel == null)    
        {
            _logger.LogInformation($"Consumer channel created.");
            _consumerChannel = GenerateConsumerConnection(name).CreateModel();
        }
        _logger.LogInformation($"Consumer channel acquired.");
        return _consumerChannel;
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
        return _publisherConnections.GetOrAdd(name, GeneratePublisherChannel(name));
    }
}