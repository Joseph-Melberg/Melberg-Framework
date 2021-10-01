using Melberg.Core.Redis;
using Melberg.Infrastructure.Redis;

namespace Demo.Redis.Infrastructure
{
    public class RedisDemoContext : RedisContext
    {
        public RedisDemoContext(IRedisConfigurationProvider provider) : base(provider)
        {
        }
    }
}