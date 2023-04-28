using System.Linq;
using MelbergFramework.Infrastructure.MySql;
namespace Demo.MySql.Infrastructure;

public class TestRepository : BaseRepository<ReadWriteContext>, ITestRepository
{
    public TestRepository(ReadWriteContext context) : base(context) {}
    public int Get()
    {
        return Context.Heartbeats.Count();
    }
    public void Test()
    {
        var j = Context.Heartbeats.First(_ => _.name == "skywatcher");
        var k = new HeartbeatModel()
        {
            announced = false,
            mac = j.mac,
            name = j.name,
            online = j.online,
            timestamp = j.timestamp
        };

        Context.Update(k);
        Context.SaveChanges();
        var l = Context.Heartbeats.First(_ => _.name == "skywatcher");

    }
}