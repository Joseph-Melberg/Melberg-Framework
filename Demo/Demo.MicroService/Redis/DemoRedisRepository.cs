using System.Diagnostics;
using MelbergFramework.Infrastructure.Redis.Repository;
using StackExchange.Redis;

namespace Demo.Microservice.Redis;

public interface IDemoRedisRepository
{
    void Test();
    Task Test2();
}

public class DemoRedisRepository : RedisRepository<DemoRedisContext>, IDemoRedisRepository
{
    private readonly DemoRedisContext _context;
    private IDatabaseAsync _db => _context.DB;
    public DemoRedisRepository(DemoRedisContext context) : base(context)
    {
        _context = context;
    }
    
    public void Test()
    {
        var ammount =4000;
        var j = new List<Task>();
        for(int i = 0; i < ammount; i++)
        {
            j.Add(Test1()) ;
        }
        
        var stopw = new Stopwatch();
        
        stopw.Start();
        
        Task.WhenAll(j).Wait();
        
        stopw.Stop();
        Console.WriteLine(stopw.ElapsedMilliseconds);
        var k = new List<Task>();
        for(int i = 0; i < ammount; i++)
        {
            k.Add(Test2());
        }
        stopw.Restart();
        Task.WhenAll(k).Wait();
        
        stopw.Stop();

        Console.WriteLine(stopw.ElapsedMilliseconds);
    }
    
    public async Task Test2()
    {
        await _db.StringSetAsync("abcde","a",TimeSpan.FromSeconds(10));
        await _db.StringGetAsync("abcde");
    }
    public async Task Test1()
    {
        await DB.StringSetAsync("abcde","a",TimeSpan.FromSeconds(10));
        await DB.StringGetAsync("abcde");
    }
}