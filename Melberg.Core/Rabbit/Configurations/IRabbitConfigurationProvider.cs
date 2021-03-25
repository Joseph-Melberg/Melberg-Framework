using System.Collections.Generic;
using Melberg.Core.Rabbit.Configurations.Data;

namespace Melberg.Core.Rabbit.Configurations
{
    public interface IRabbitConfigurationProvider
    {
        IEnumerable<ConnectionFactoryConfigData> GetConnectionConfigData();

        ConnectionFactoryConfigData GetConnectionConfigData(string connection);

        PublisherConfigData GetPublisherConfiguration(string publisherName);

        AmqpObjectsDeclarationConfigData GetAmqpObjectsConfiguration();

        AsyncReceiverConfigData GetAsyncReceiverConfiguration(string receiverName);
    }
}