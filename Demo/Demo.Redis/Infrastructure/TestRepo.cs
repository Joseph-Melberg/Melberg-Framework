using System;
using System.Threading.Tasks;
using Demo.Redis.Infrastructure.Core;
using MelbergFramework.Infrastructure.Redis.Repository;

namespace Demo.Redis.Infrastructure
{
    public class TestRepo : RedisRepository<RedisDemoContext>, ITestRepo
    {
        public TestRepo(RedisDemoContext context) : base(context) {}
        
        public async Task Set(string value)
        {
            await DB.StringSetAsync("Va",value);
        }

        public async Task<string> Get(string key)
        {
            return await DB.StringGetAsync(key);
        }

        public void ListKeys()
        {
            foreach(var key in Server.Keys(pattern: "*"))
            {
                Console.WriteLine(key);
            }
        }
    }
}