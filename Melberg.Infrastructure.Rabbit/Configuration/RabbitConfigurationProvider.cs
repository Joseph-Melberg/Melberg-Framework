using System.Collections.Generic;
using System.Linq;
using Melberg.Core.Rabbit.Configurations;
using Melberg.Core.Rabbit.Configurations.Data;
using Microsoft.Extensions.Configuration;

namespace Melberg.Infrastructure.Rabbit.Configuration
{
    public class RabbitConfigurationProvider : IRabbitConfigurationProvider
    {
        private readonly IConfiguration _configuration;
        public RabbitConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public AmqpObjectsDeclarationConfigData GetAmqpObjectsConfiguration()
        {
            throw new System.NotImplementedException();
        }

        public AsyncReceiverConfigData GetAsyncReceiverConfiguration(string receiverName)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ConnectionFactoryConfigData> GetConnectionConfigData()
        {
            throw new System.NotImplementedException();
        }

        public ConnectionFactoryConfigData GetConnectionConfigData(string connection)
        {
            throw new System.NotImplementedException();
        }

        public PublisherConfigData GetPublisherConfiguration(string publisherName)
        {
            throw new System.NotImplementedException();
        }

        AmqpObjectsDeclarationConfigData IRabbitConfigurationProvider.GetAmqpObjectsConfiguration()
        {
            var result = new AmqpObjectsDeclarationConfigData();

            result.ExchangeList = _configuration.GetSection("Rabbit:ServerDeclarations:Exchanges").Get<ExchangeConfigData[]>();
            result.BindingList = _configuration.GetSection("Rabbit:ServerDeclarations:Bindings").Get<BindingConfigData[]>(); 
            result.QueueList = _configuration.GetSection("Rabbit:ServerDeclarations:Queues").Get<QueueConfigData[]>();

            return result;
        }

        AsyncReceiverConfigData IRabbitConfigurationProvider.GetAsyncReceiverConfiguration(string receiverName)
        {
            if(receiverName == null)
            {
                throw new System.Exception("Reciever Name not given");
            }
            var section =_configuration
            .GetSection("Rabbit:ClientDeclarations:AsyncRecievers").Get<AsyncReceiverConfigData[]>()
            
            .Where(_ => _.Name == "IncomingMessages").First(); 
        
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

        ConnectionFactoryConfigData IRabbitConfigurationProvider.GetConnectionConfigData(string connection)
        {
            if(connection == null)
            {
                throw new System.Exception("Connection not given");
            }

            return _configuration.GetSection("Rabbit:ClientDeclarations:Connections").Get<ConnectionFactoryConfigData[]>().Where(_ => _.Name == connection).First();
        }

        PublisherConfigData IRabbitConfigurationProvider.GetPublisherConfiguration(string publisherName)
        {
            throw new System.NotImplementedException();
        }
    }
}