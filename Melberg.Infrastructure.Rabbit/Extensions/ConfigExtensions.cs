using System.Collections.Generic;
using Melberg.Core.Rabbit.Configurations.Data;

namespace Melberg.Infrastructure.Rabbit.Extensions
{
    public static class ConfigExtensions
    {
        public static Dictionary<string, object> GetQueueArgs(this QueueConfigData queue)
        {
            var args = new Dictionary<string, object>();

            if (queue.MessageTtl > 0)
            {
                args.Add("x-message-ttl", queue.MessageTtl);
            }

            if (!string.IsNullOrEmpty(queue.DeadLetterExchange))
            {
                args.Add("x-dead-letter-exchange", queue.DeadLetterExchange);
            }

            //Override rabbit queue location client-local, random, or min-masters(default)
            //https://www.rabbitmq.com/ha.html
            if (!string.IsNullOrEmpty(queue.QueueMasterLocatorSetting))
            {
                args.Add("x-queue-master-locator", queue.QueueMasterLocatorSetting);
            }

            return args;
        }
    }
}