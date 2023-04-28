namespace MelbergFramework.Core.MySql;
public interface IMySqlConnectionStringProvider
{
    string GetConnectionString(string contextName);
}