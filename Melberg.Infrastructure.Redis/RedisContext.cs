using Melberg.Core.Redis;
using StackExchange.Redis;

namespace Melberg.Infrastructure.Redis
{
    public class RedisContext
    {
        private ConnectionMultiplexer _connection;
        public RedisContext(IRedisConfigurationProvider provider)
        {
            _connection = ConnectionMultiplexer.Connect(provider.GetConnectionString(this.GetType().Name));
        }

        public IDatabaseAsync DB => _connection.GetDatabase();

    }
}