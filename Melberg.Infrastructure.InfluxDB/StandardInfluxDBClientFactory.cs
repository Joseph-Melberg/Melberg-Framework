using System.Collections.Generic;
using InfluxDB.Client;
using Melberg.Common.Exceptions;

namespace Melberg.Infrastructure.InfluxDB;

public class StandardInfluxDBClientFactory : IStandardInfluxDBClientFactory
{
    private readonly InfluxDBConfigurationProvider _configurationProvider;
    private readonly Dictionary<string,InfluxDBClient> _clients = new Dictionary<string, InfluxDBClient>();
    
    public StandardInfluxDBClientFactory(InfluxDBConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;
    }
    public InfluxDBClient GetClient(string name)
    {
        if(!_clients.ContainsKey(name))
        {
            var connectionString = _configurationProvider.GetConnectionString(this.GetType().Name)
            ?? throw new MissingConnectionStringException($"Unable to find connection string: {this.GetType().Name}");

            _clients.Add(name, InfluxDBClientFactory.Create(connectionString));
        }
        return _clients[name];
    }
}