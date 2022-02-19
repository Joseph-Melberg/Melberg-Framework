using System;
using System.Text;
using Melberg.Core.Rabbit.Configurations;
using Melberg.Core.Rabbit.Configurations.Data;
using Melberg.Infrastructure.Rabbit.Configuration;
using Melberg.Infrastructure.Rabbit.Messages;
using RabbitMQ.Client;

namespace Melberg.Infrastructure.Rabbit.Publishers;

public abstract class BasePublisher<TMessage> : IDisposable
    where TMessage :  IStandardMessage
{
    private IModel _channel;
    protected IModel Channel 
    {
        get
        {
            if(_channel == null)
            {
                try
                {
                    _channel = _connectionFactory.GetConnection().CreateModel();
                }
                catch (System.Exception)
                {
                    throw;
                }
            }
            return _channel;
        }
    }
    private readonly StandardConnectionFactory _connectionFactory;
    private readonly PublisherConfigData _config;
    private bool _disposed;

    public BasePublisher(IRabbitConfigurationProvider configurationProvider)
    {
        _config = configurationProvider.GetPublisherConfiguration(typeof(TMessage).Name);
        var connectionConfig = configurationProvider.GetConnectionConfigData(_config.Connection);
        _connectionFactory = new StandardConnectionFactory(connectionConfig);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Emit(Message message)
    {
        var properties = Channel.CreateBasicProperties();

        properties.Headers = message.Headers;

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
        if (disposing)
        {
            _channel.Close();
        }

        _disposed = true;
    }
}