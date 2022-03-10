using Melberg.Core.InfluxDB;
using Melberg.Infrastructure.InfluxDB;

namespace Demo.InfluxDB.Infrastructure;

public class TestInfluxDBContext : DefaultContext
{
    public TestInfluxDBContext(IInfluxDBConfigurationProvider configurationProvider) : base(configurationProvider) { }
}