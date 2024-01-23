using System;
using MelbergFramework.Core.Redis;
using StackExchange.Redis;

namespace MelbergFramework.Infrastructure.Redis;

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
        _connection = ConnectionMultiplexer.Connect(connectionString);
        
        //StackExchange had to have a good reason to think that this was how 
        //their code should work.

        //I am unaware of thier reasoning, I do not understand why this must
        //be done.
        var serverSelector = connectionString.Split(',')[0];

        Server = _connection.GetServer(serverSelector);
    }

    public IServer Server {get; private set;}
    public IDatabaseAsync DB => _connection.GetDatabase();
}
