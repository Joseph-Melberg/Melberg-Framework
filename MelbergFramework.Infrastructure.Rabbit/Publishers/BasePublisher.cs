using System;
using System.Diagnostics;
using MelbergFramework.Core.Rabbit.Configurations;
using MelbergFramework.Core.Rabbit.Configurations.Data;
using MelbergFramework.Infrastructure.Rabbit.Factory;
using MelbergFramework.Infrastructure.Rabbit.Messages;
using RabbitMQ.Client;

namespace MelbergFramework.Infrastructure.Rabbit.Publishers;

public abstract class BasePublisher<TMessage>
    where TMessage :  IStandardMessage
{
    private IModel _channel;
    protected IModel Channel 
    {
        get
        {
            if(_channel == null)
            {
                _channel = _connectionFactory.GetPublisherChannel(typeof(TMessage).Name).CreateModel();
            }
            return _channel;
        }
    }

    private readonly IStandardConnectionFactory _connectionFactory;
    private readonly PublisherConfigData _config;
    private bool _disposed;


    public BasePublisher(IRabbitConfigurationProvider configurationProvider)
    {
        _config = configurationProvider.GetPublisherConfiguration(typeof(TMessage).Name);
        _connectionFactory = new StandardConnectionFactory(configurationProvider);
    }


    public void Emit(Message message)
    {
        var properties = Channel.CreateBasicProperties();
        
        properties.Headers = message.Headers;
        
        properties.Headers[Messages.Headers.Timestamp] = DateTime.UtcNow.ToString();
        properties.Headers[Messages.Headers.CorrelationId]= Trace.CorrelationManager.ActivityId.ToString();

        Channel.BasicPublish(
            _config.Exchange,
            message.RoutingKey,
            true,
            properties,
            message.Body);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }
        if (disposing && _channel != null)
        {
            _channel.Close();
        }

        _disposed = true;
    }
}