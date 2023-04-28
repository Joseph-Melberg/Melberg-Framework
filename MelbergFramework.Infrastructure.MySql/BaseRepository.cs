namespace MelbergFramework.Infrastructure.MySql;
public class BaseRepository<TContext>
    where TContext : DefaultContext
{
    protected TContext Context;
    public BaseRepository(TContext context)
    {
        Context = context;
    }
}