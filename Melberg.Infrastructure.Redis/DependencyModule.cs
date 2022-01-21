using Melberg.Core.Redis;
using Melberg.Infrastructure.Redis.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Melberg.Infrastructure.Redis;
public static class RedisModule
{
    
    public static void LoadRedisRepository<TFrom, TTo, TContext>(IServiceCollection catalog)
        where TTo : RedisRepository<TContext>,TFrom
        where TFrom : class
        where TContext : RedisContext
    {
        catalog.AddSingleton<IRedisConfigurationProvider, RedisConfigurationProvider>();

        catalog.AddTransient<TFrom, TTo>();

        catalog.AddSingleton<TContext, TContext>();
    }
}