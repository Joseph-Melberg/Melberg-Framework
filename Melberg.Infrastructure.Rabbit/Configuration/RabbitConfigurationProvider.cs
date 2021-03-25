using System.Collections.Generic;
using Melberg.Core.Rabbit.Configurations;
using Melberg.Core.Rabbit.Configurations.Data;
using Melberg.Infrastructure.Rabbit.Models;
using Microsoft.Extensions.Configuration;

namespace Melberg.Infrastructure.Rabbit.Configuration
{
    public class RabbitConfigurationProvider : IRabbitConfigurationProvider
    {
        private readonly RabbitConfigurationProvider _rabbitConfig;

        public RabbitConfigurationProvider(IConfiguration config)
        {
            _rabbitConfig = config.GetSection(RabbitConfiguration.ConfigurationName).Get<RabbitConfiguration>();
        }

        protected virtual string GetQueueName(string queueName) => queueName;

        public IEnumerable<ConnectionFactoryConfigData> GetConnectionConfigData()
        {
            if (_rabbitConfig?.AMQPConnections?.ConnectionSettings?.ConnectionList == null)

            var configData = _rabbitConfig.AmqpConnections.ConnectionSettings.Select( _ =>
            new ConnectionFactoryConfigData()
            {
                Name = _.Name,
                Server = _.ServerName,
                UserName = _.UserName,
                Password = _.Password,
                MaxConcurrentChannels = _.MaxConcurrentChannels,
                UseSsl = _.UseSsl
            }
            );

            return configData;
        }

        public ConnectionFactoryConfigData GetConnectionConfigData(string connection)
        {
            var connectionName = GetConnectionConfigData().SingleOrDefault(_ => _.Name == connection);
            if(connectionName == null)
            {
                 
            }
        }

        public PublisherConfigData GetPublisherConfiguration(string publisherName)
        {
            throw new System.NotImplementedException();
        }

        public AmqpObjectsDeclarationConfigData GetAmqpObjectsConfiguration()
        {
            throw new System.NotImplementedException();
        }

        public AsyncReceiverConfigData GetAsyncReceiverConfiguration(string receiverName)
        {
            throw new System.NotImplementedException();
        }            throw new MyQApplicationConfigurationException($"RabbitMQ Connection configuration not found.");
            }

            var configData = _rabbitConfiguration.AMQPConnections.ConnectionSettings.ConnectionList.Select(_ =>
                new ConnectionFactoryConfigData()
                {
                    Name = _.Name,
                    Server = _.ServerName,
                    UserName = _.UserName,
                    Password = _.Password,
                    MaxConcurrentChannels = _.MaxConcurrentChannels,
                    UseSsl = _.UseSsl
                }
                );

            return configData; 
        } 
    
    }
}