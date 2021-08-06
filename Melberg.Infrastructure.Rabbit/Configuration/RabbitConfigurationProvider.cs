using System.Collections.Generic;
using Melberg.Core.Rabbit.Configurations;
using Melberg.Core.Rabbit.Configurations.Data;

namespace Melberg.Infrastructure.Rabbit.Configuration
{
    public class RabbitConfigurationProvider : IRabbitConfigurationProvider
    {
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
            throw new System.NotImplementedException();
        }

        AsyncReceiverConfigData IRabbitConfigurationProvider.GetAsyncReceiverConfiguration(string receiverName)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<ConnectionFactoryConfigData> IRabbitConfigurationProvider.GetConnectionConfigData()
        {
            throw new System.NotImplementedException();
        }

        ConnectionFactoryConfigData IRabbitConfigurationProvider.GetConnectionConfigData(string connection)
        {
            throw new System.NotImplementedException();
        }

        PublisherConfigData IRabbitConfigurationProvider.GetPublisherConfiguration(string publisherName)
        {
            throw new System.NotImplementedException();
        }
    }
}