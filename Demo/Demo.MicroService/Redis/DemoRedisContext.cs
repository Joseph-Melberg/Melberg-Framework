using MelbergFramework.Core.Redis;
using MelbergFramework.Infrastructure.Redis;

namespace Demo.Microservice.Redis;

public class DemoRedisContext : RedisContext
{
    public DemoRedisContext(IRedisConfigurationProvider provider) : base(provider)
    {
    }
}