using Demo.Redis.Infrastructure;
using Demo.Redis.Infrastructure.Core;
using MelbergFramework.Infrastructure.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Redis
{
    public class Register
    {

        public static ServiceCollection RegisterServices(ServiceCollection services)
        {
            RedisModule.LoadRedisRepository<ITestRepo,TestRepo,RedisDemoContext>(services);
            return services;
        }
    }
}