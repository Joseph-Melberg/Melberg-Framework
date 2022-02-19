using System;
using Melberg.Core.Redis;
using StackExchange.Redis;

namespace Melberg.Infrastructure.Redis;

public class RedisContext
{
    private ConnectionMultiplexer _connection;
    public RedisContext(IRedisConfigurationProvider provider)
    {
        string connectionString = null;
        connectionString = provider.GetConnectionString(this.GetType().Name);
        if(string.IsNullOrEmpty(connectionString))
        {
            throw new Exception($"Connection string for {this.GetType().Name} is missing");
        }
        
        _connection = ConnectionMultiplexer.Connect(provider.GetConnectionString(this.GetType().Name));
        DB = _connection.GetDatabase();
    }

    public IDatabaseAsync DB{ get; private set;}
}