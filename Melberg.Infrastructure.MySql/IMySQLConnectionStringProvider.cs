namespace Melberg.Infrastructure.MySql
{
    public interface IMySQLConnectionStringProvider
    {
        string GetConnectionString(string contextName);
    }
}