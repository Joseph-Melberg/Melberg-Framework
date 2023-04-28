using StackExchange.Redis;

namespace MelbergFramework.Infrastructure.Redis.Repository;
public class RedisRepository<TContext>
    where TContext : RedisContext
{

    public IDatabaseAsync DB {get; private set;}
    public IServer Server {get; private set;}
    public RedisRepository(TContext context)
    {
        DB = context.DB;
        Server = context.Server;
    }
}