namespace MelbergFramework.Core.InfluxDB;

public interface IInfluxDBConfigurationProvider
{
    string GetConnectionString(string contextName);
}