using System.Threading.Tasks;

namespace Demo.Redis.Infrastructure.Core
{
    public interface ITestRepo
    {
        Task<string> Get(string value);
        
        Task Set(string value);
    }
}