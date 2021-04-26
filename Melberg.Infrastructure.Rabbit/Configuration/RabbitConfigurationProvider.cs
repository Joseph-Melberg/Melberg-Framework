using System.Collections.Generic;
using Melberg.Core.Rabbit.Configurations;
using Melberg.Core.Rabbit.Configurations.Data;
using Melberg.Infrastructure.Rabbit.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;

namespace Melberg.Infrastructure.Rabbit.Configuration
{
    public class RabbitConfigurationProvider : IRabbitConfigurationProvider
    {
        private readonly RabbitConfiguration _rabbitConfig;

        public RabbitConfigurationProvider(IConfiguration config)
        {
            _rabbitConfig = config.GetSection(RabbitConfiguration.ConfigurationName).Get<RabbitConfiguration>();
        }

        protected virtual string GetQueueName(string queueName) => queueName;

        public IEnumerable<ConnectionFactoryConfigData> GetConnectionConfigData()
        {
            if (_rabbitConfig?.AmqpConnections?.ConnectionSettings?.ConnectionList == null)
            {
                throw new System.Exception("RabbitMQ Configuration not found");
            }
            var configData = _rabbitConfig.AmqpConnections.ConnectionSettings?.ConnectionList.Select( _ =>
            new ConnectionFactoryConfigData()
            {
                Name = _.Name,
                ServerName = _.ServerName,
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
                throw new System.Exception($"RabbitMQ Connection with name {connection} not found");
            }
            return connectionName;
        }

        public PublisherConfigData GetPublisherConfiguration(string publisherName)
        {
        
            var publisher = _rabbitConfig?.AmqpConnections?.ConnectionSettings?.PublisherList.SingleOrDefault(_ => _.Name == publisherName);
            if (publisher == null)
            {
                throw new System.Exception(
                    $"Rabbit Publisher configuration for {publisherName} not found");
            }

            return new PublisherConfigData()
            {
                Name = publisher.Name,
                Connection = publisher.Connection,
                Exchange = publisher.Exchange,
                Immediate = publisher.Immediate,
                Mandatory = publisher.Mandatory,
                Type = publisher.Type
            };
        }

        public AmqpObjectsDeclarationConfigData GetAmqpObjectsConfiguration()
        {
                        if (_rabbitConfig?.AmqpServerObjects?.AmqpObjectsDeclaration == null)
            {
                return null;
            }

            var exchangeList = _rabbitConfig.AmqpServerObjects.AmqpObjectsDeclaration.ExchangeList.Select(
                _ =>
                    new ExchangeConfigData()
                    {
                        AutoDelete = _.AutoDelete,
                        Connection = _.Connection,
                        Durable = _.Durable,
                        Name = _.Name,
                        Type = (ExchangeConfigType)Enum.Parse(typeof(ExchangeConfigType), _.Type)
                    }).ToList();

            ValidateQueueMasterLocatorSetting();
            var queueList = _rabbitConfig.AmqpServerObjects.AmqpObjectsDeclaration.QueueList.Select(
            _ =>
                new QueueConfigData()
                {
                    AutoDelete = _.AutoDelete,
                    Connection = _.Connection,
                    DeadLetterExchange = _.DeadLetterExchange,
                    Durable = _.Durable,
                    Exclusive = _.Exclusive,
                    MessageTtl = _.MessageTtl,
                    QueueMasterLocatorSetting = _.QueueMasterLocatorSetting,
                    Name = GetQueueName(_.Name),
                }).ToList();

            var bindingList = _rabbitConfig.AmqpServerObjects.AmqpObjectsDeclaration.BindingList.Select(
                _ =>
                    new BindingConfigData()
                    {
                        Connection = _.Connection,
                        Exchange = _.Exchange,
                        Queue = GetQueueName(_.Queue),
                        SubscriptionKey = _.SubscriptionKey
                    }).ToList();

            var configData = new AmqpObjectsDeclarationConfigData()
            {
                BindingList = bindingList,
                ExchangeList = exchangeList,
                QueueList = queueList
            };

            return configData;
        }
        private void ValidateQueueMasterLocatorSetting()
        {
            var validQueueMasterLocatorSetting = new string[] {"client-local", "random", "min-masters","",null};
            foreach (var setting in _rabbitConfig?.AmqpServerObjects?.AmqpObjectsDeclaration?.QueueList.Select(_=>_?.QueueMasterLocatorSetting))
            {
                if (!validQueueMasterLocatorSetting.Contains(setting))
                {
                    throw new System.Exception(
                        $"Rabbit Queue invalid configuration for QueueMasterLocatorSetting: {setting}. Must be client-local, random, or min-masters");
                }
            }
        }
        
        public AsyncReceiverConfigData GetAsyncReceiverConfiguration(string receiverName)
        {
            var receiver = _rabbitConfig?.AmqpConnections?.ConnectionSettings?.AsyncReceiverList?.SingleOrDefault(_ => _.Name == receiverName);
            if (receiver == null)
            {
                throw new System.Exception($"Rabbit Receiver configuration for {receiverName} not found");
            }

            return new AsyncReceiverConfigData()
            {
                Connection = receiver.Connection,
                MaxThreads = receiver.MaxThreads,
                Name = receiver.Name,
                Prefetch = receiver.Prefetch,
                Queue = GetQueueName(receiver.Queue)
            };
        }
    }
}