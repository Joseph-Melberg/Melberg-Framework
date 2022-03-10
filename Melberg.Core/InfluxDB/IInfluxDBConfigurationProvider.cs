namespace Melberg.Core.InfluxDB;

public interface IInfluxDBConfigurationProvider
{
    string GetConnectionString(string contextName);
}