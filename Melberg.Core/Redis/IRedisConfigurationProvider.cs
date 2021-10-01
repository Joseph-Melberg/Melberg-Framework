using Melberg.Core.Redis;

namespace Melberg.Core.Redis

{
    public interface IRedisConfigurationProvider
    {
        string GetConnectionString(string connectionStringName);
    }
}