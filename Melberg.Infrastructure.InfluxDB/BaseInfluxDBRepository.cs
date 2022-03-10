namespace Melberg.Infrastructure.InfluxDB;

public class BaseInfluxDBRepository<TContext>
{
    protected TContext Context;
    public BaseInfluxDBRepository(TContext context)
    {
        Context = context;
    }
}