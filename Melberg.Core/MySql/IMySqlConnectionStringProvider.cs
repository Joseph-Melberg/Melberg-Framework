namespace Melberg.Core.MySql
{
    public interface IMySqlConnectionStringProvider
    {
        string GetConnectionString(string contextName);
    }
}