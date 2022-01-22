using Melberg.Infrastructure.MySql;
namespace Demo.MySql.Infrastructure;

public class TestRepository : BaseRepository<ReadWriteContext>, ITestRepository
{
    public TestRepository(ReadWriteContext context) : base(context) {}
    public int Get()
    {
        return Context.Heartbeats.Count();
    }
}