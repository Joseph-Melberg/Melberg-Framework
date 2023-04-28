using System;
using System.Collections.Concurrent;
using System.Threading;
using MelbergFramework.Core.Redis;
using Microsoft.Extensions.Configuration;

namespace MelbergFramework.Infrastructure.Redis;

public class RedisConfigurationProvider : IRedisConfigurationProvider
{
    private readonly IConfiguration _configuration;
    private readonly ConcurrentDictionary<string, Lazy<string>> _connectionStrings = new ConcurrentDictionary<string, Lazy<string>>();

    public RedisConfigurationProvider(IConfiguration Configuration) 
    {
        _configuration = Configuration;
    }
    public string GetConnectionString(string connectionStringName)
    {
        var connectionString = _connectionStrings.GetOrAdd(connectionStringName,
            connecStringName => new Lazy<string>(() => _configuration.GetConnectionString(connecStringName),
                LazyThreadSafetyMode.ExecutionAndPublication));

        return connectionString.Value;
    }
}