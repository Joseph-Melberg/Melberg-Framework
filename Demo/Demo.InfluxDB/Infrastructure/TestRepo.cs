using Demo.InfluxDB.Infrastructure.Core;
using Melberg.Infrastructure.InfluxDB;

namespace Demo.InfluxDB.Infrastructure;

public class TestRepo : BaseInfluxDBRepository<TestInfluxDBContext>, ITestRepo
{
    public TestRepo(TestInfluxDBContext context) : base(context) { }

    public async Task Test()
    {
        var test = new InfluxDBDataModel("temperature");
        test.Tags.Add("hostname","test");
        test.Fields.Add("value", 40.3D);

        
        await Context.WritePointAsync(test,"temperature","Inter");
    }
}