using System.Collections.Generic;

namespace Melberg.Infrastructure.Rabbit.Configuration.Models
{
    public class ConnectionSettings
    {
        public List<RabbitConnection> ConnectionList {get; set;}

        public List<RabbitPublisher> PublisherList {get; set;}

        public List<AsyncReceiver> AsyncReceiverList {get; set;} 
    }
}