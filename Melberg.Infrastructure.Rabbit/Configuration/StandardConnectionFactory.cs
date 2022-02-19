using Melberg.Core.Rabbit.Configurations.Data;
using RabbitMQ.Client;

namespace Melberg.Infrastructure.Rabbit.Configuration;

public class StandardConnectionFactory 
{
    private ConnectionFactory _factory;
    public StandardConnectionFactory(ConnectionFactoryConfigData connectionConfig)
    {
        
        _factory = new ConnectionFactory();
        _factory.UserName = connectionConfig.UserName;
        _factory.Password = connectionConfig.Password;
        _factory.VirtualHost = "/";
        _factory.DispatchConsumersAsync = true;
        _factory.HostName = connectionConfig.ServerName;
        _factory.ClientProvidedName = connectionConfig.ClientName;
    }    

    public IConnection GetConnection() => _factory.CreateConnection(); 
}