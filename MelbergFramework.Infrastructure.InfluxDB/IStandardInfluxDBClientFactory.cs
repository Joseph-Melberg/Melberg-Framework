using InfluxDB.Client;

namespace MelbergFramework.Infrastructure.InfluxDB;

public interface IStandardInfluxDBClientFactory
{
    public InfluxDBClient GetClient(string name);
}