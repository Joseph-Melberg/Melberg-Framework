using System.Collections.Generic;
using InfluxDB.Client;
using Melberg.Common.Exceptions;
using Melberg.Core.InfluxDB;

namespace Melberg.Infrastructure.InfluxDB;

public class StandardInfluxDBClientFactory : IStandardInfluxDBClientFactory
{
    private readonly IInfluxDBConfigurationProvider _configurationProvider;
    private readonly Dictionary<string,InfluxDBClient> _clients = new Dictionary<string, InfluxDBClient>();
    
    public StandardInfluxDBClientFactory(IInfluxDBConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;
    }
    public InfluxDBClient GetClient(string name)
    {
        if(!_clients.ContainsKey(name))
        {
            var connectionString = _configurationProvider.GetConnectionString(name)
            ?? throw new MissingConnectionStringException($"Unable to find connection string: {name}");

            _clients.Add(name, InfluxDBClientFactory.Create(connectionString));
        }
        return _clients[name];
    }
}