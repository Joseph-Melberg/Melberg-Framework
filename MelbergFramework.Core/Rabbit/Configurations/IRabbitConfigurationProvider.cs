using System.Collections.Generic;
using MelbergFramework.Core.Rabbit.Configurations.Data;

namespace MelbergFramework.Core.Rabbit.Configurations;
public interface IRabbitConfigurationProvider
{
    IEnumerable<ConnectionFactoryConfigData> GetConnectionConfigData();

    ConnectionFactoryConfigData GetConnectionConfigData(string connection);

    PublisherConfigData GetPublisherConfiguration(string publisherName);

    AmqpObjectsDeclarationConfigData GetAmqpObjectsConfiguration();

    AsyncReceiverConfigData GetAsyncReceiverConfiguration(string receiverName);
}