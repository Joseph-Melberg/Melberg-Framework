using MelbergFramework.Core.InfluxDB;
using MelbergFramework.Infrastructure.InfluxDB;

namespace Demo.InfluxDB.Infrastructure;

public class TestInfluxDBContext : DefaultContext
{
    public TestInfluxDBContext(IStandardInfluxDBClientFactory factory) : base(factory) { }
}