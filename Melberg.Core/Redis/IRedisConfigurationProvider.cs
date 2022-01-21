namespace Melberg.Core.Redis;
public interface IRedisConfigurationProvider
{
    string GetConnectionString(string connectionStringName);
}