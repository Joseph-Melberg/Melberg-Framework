namespace Melberg.Infrastructure.Rabbit.Connection
{
    public interface IServerConfigurator
	{
		void InitializeConfiguredObjects(IConnection connection, string connectionName);
	}
}