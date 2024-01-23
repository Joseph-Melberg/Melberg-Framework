using StackExchange.Redis;

namespace MelbergFramework.Infrastructure.Redis.Repository;
public class RedisRepository<TContext>
    where TContext : RedisContext
{
    private readonly TContext _context;

    public IDatabaseAsync DB  => _context.DB;
    public IServer Server => _context.Server;
    public RedisRepository(TContext context) { _context = context; }
}