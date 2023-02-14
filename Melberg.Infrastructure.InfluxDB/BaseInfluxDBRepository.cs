namespace Melberg.Infrastructure.InfluxDB;

public class BaseInfluxDBRepository<TContext>
    where TContext : DefaultContext
{
    protected TContext Context;
    public BaseInfluxDBRepository(TContext context)
    {
        Context = context;
    }
}