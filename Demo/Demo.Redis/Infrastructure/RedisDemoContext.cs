using MelbergFramework.Core.Redis;
using MelbergFramework.Infrastructure.Redis;

namespace Demo.Redis.Infrastructure
{
    public class RedisDemoContext : RedisContext
    {
        public RedisDemoContext(IRedisConfigurationProvider provider) : base(provider)
        {
        }
    }
}