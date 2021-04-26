using Chamberlain.Middleware.Infrastructure.Rabbit.Connection;

namespace Melberg.Infrastructure.Rabbit.Connection
{
        public class RabbitChannelFactory : IChannelFactory
    {
        private readonly IConnectionFactory _connectionFactory;

        public RabbitChannelFactory(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        
        public IChannel GetChannel(string connectionName)
        {
            var connection = _connectionFactory.GetConnection(connectionName);
            return connection.CreateChannel();
        }

        public void ReleaseChannel(IChannel channel)
        {
            channel?.Connection?.ReleaseChannel(channel);
        }
    }
}