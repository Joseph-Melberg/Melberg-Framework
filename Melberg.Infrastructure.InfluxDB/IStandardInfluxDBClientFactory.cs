using InfluxDB.Client;

namespace Melberg.Infrastructure.InfluxDB;

public interface IStandardInfluxDBClientFactory
{
    public InfluxDBClient GetClient(string name);
}