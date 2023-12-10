using System.Collections.Generic;
using System.Linq;
using MelbergFramework.Core.Rabbit.Configurations;
using MelbergFramework.Core.Rabbit.Configurations.Data;
using Microsoft.Extensions.Configuration;

namespace MelbergFramework.Infrastructure.Rabbit.Configuration;
public class RabbitConfigurationProvider : IRabbitConfigurationProvider
{
    private readonly IConfiguration _configuration;
    public RabbitConfigurationProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<ConnectionFactoryConfigData> GetConnectionConfigData()
    {
        throw new System.NotImplementedException();
    }

    public PublisherConfigData GetPublisherConfiguration(string publisherName)
    {
        if(publisherName == null)
        {
            throw new System.Exception("Reciever Name not given");
        }
        return _configuration
        .GetSection("Rabbit:ClientDeclarations:Publishers").Get<PublisherConfigData[]>()
        .Where(_ => _.Name == publisherName).First();
    }

    public AmqpObjectsDeclarationConfigData GetAmqpObjectsConfiguration()
    {
        var result = new AmqpObjectsDeclarationConfigData();

        result.ExchangeList = _configuration.GetSection("Rabbit:ServerDeclarations:Exchanges").Get<ExchangeConfigData[]>();
        result.BindingList = _configuration.GetSection("Rabbit:ServerDeclarations:Bindings").Get<BindingConfigData[]>(); 
        result.QueueList = _configuration.GetSection("Rabbit:ServerDeclarations:Queues").Get<QueueConfigData[]>();

        return result;
    }

    public AsyncReceiverConfigData GetAsyncReceiverConfiguration(string receiverName)
    {
        if(receiverName == null)
        {
            throw new System.Exception("Reciever Name not given");
        }
        var section =_configuration
        .GetSection("Rabbit:ClientDeclarations:AsyncRecievers").Get<AsyncReceiverConfigData[]>()
        
        .Where(_ => _.Name == receiverName).First(); 
    
        return new AsyncReceiverConfigData
        {
            Connection = section.Connection,
            Name = section.Name,
            Queue = section.Queue
        };
    }

    IEnumerable<ConnectionFactoryConfigData> IRabbitConfigurationProvider.GetConnectionConfigData()
    {
        throw new System.NotImplementedException();
    }

    public ConnectionFactoryConfigData GetConnectionConfigData(string connection)
    {
        if(connection == null)
        {
            throw new System.Exception("Connection not given");
        }

        return _configuration.GetSection("Rabbit:ClientDeclarations:Connections").Get<ConnectionFactoryConfigData[]>().Where(_ => _.Name == connection).First();
    }
}