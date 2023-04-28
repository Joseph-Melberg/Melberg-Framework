namespace MelbergFramework.Core.Redis;
public interface IRedisConfigurationProvider
{
    string GetConnectionString(string connectionStringName);
}