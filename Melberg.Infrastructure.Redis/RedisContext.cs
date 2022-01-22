using System;
using Melberg.Core.Redis;
using StackExchange.Redis;

namespace Melberg.Infrastructure.Redis;

public class RedisContext
{
    private ConnectionMultiplexer _connection;
    public RedisContext(IRedisConfigurationProvider provider)
    {
        try
        {
            var w = 3;    
            Console.WriteLine(provider.GetConnectionString(this.GetType().Name));
            _connection = ConnectionMultiplexer.Connect(provider.GetConnectionString(this.GetType().Name));
            DB = _connection.GetDatabase();
        }
        catch(Exception ex)
        {
            var j = 3;
        }

    }

    public IDatabaseAsync DB{ get; private set;}
}