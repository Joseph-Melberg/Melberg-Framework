using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Chamberlain.Middleware.Infrastructure.Rabbit.Connection;
using Melberg.Core.Rabbit.Configurations;

namespace Melberg.Infrastructure.Rabbit.Connection
{
    public class RabbitConnectionFactory : IConnectionFactory
    {
        private readonly IRabbitConfigurationProvider _amqpRabbitConfigurationProvider;
        private readonly IServerConfigurator _serverConfigurator;

        private readonly Dictionary<string, RabbitMQ.Client.ConnectionFactory> _connectionFactories = new Dictionary<string, RabbitMQ.Client.ConnectionFactory>();
        private Dictionary<string, IConnection> _connections = new Dictionary<string, IConnection>();
        private static readonly object LockObject = new object();
        private bool _disposed;

        public RabbitConnectionFactory(
            IRabbitConfigurationProvider amqpRabbitConfigurationProvider,
            IServerConfigurator serverConfigurator)
        {
            _amqpRabbitConfigurationProvider = amqpRabbitConfigurationProvider;
            _serverConfigurator = serverConfigurator;

            CreateConnectionFactoryCollection();
        }

        public IConnection GetConnection(string connectionName)
        {
            if (!_connections.ContainsKey(connectionName))
            {
                return InitializeConnection(connectionName);
            }

            var connection = _connections[connectionName];
            if (connection == null || !connection.IsOpen)
            {
                connection = InitializeConnection(connectionName);
            }
            return connection;
        }

        public void EstablishConnections()
        {
            foreach (var connection in _connectionFactories)
            {
                InitializeConnection(connection.Key);
            }
        }

        private void CreateConnectionFactoryCollection()
        {
            var connections = _amqpRabbitConfigurationProvider.GetConnectionConfigData();

            foreach(var configData in connections)
            {
                var sslOption = new SslOption
                {
                    Enabled = configData.UseSsl,
                    Version = SslProtocols.Tls12,
                    ServerName = configData.ServerName
                };
                var port = configData.UseSsl ? AmqpTcpEndpoint.DefaultAmqpSslPort : AmqpTcpEndpoint.UseDefaultPort;
                
                var factory = new ConnectionFactory
                    {
                        Endpoint = new AmqpTcpEndpoint(configData.ServerName, port, sslOption),
                        UserName = configData.UserName,
                        Password = configData.Password,
                        RequestedHeartbeat = new TimeSpan(150),
                        TopologyRecoveryEnabled = false,
                        AutomaticRecoveryEnabled = false
                        
                    };
                // Load balancer timeout is 300s adjusting to keep connections open.
                _connectionFactories.Add(configData.Name, factory);
            }
        }

        private IConnection InitializeConnection(string connectionName)
        {
            if (!_connectionFactories.ContainsKey(connectionName))
            {
                throw new Exception(
                    $"A connection with the name {connectionName} does not exist in configuration, Current list of connection configuration" +
                    "{" + string.Join(",", _connectionFactories.Select(kv => kv.Key).ToArray()) + "}");
            }

            var factory = _connectionFactories[connectionName];
            lock (LockObject)
            {
                if (_connections.ContainsKey(connectionName))
                {
                    var connection = _connections[connectionName];
                    if (connection != null && connection.IsOpen)
                    {
                        _serverConfigurator.InitializeConfiguredObjects(_connections[connectionName], connectionName);
                        return connection;
                    }
                    connection?.Dispose();
                }

                RabbitMQ.Client.IConnection newRabbitMqConnection;
                try
                {
                    // Adding clientProvidedName to get additional information on Connection 
                    newRabbitMqConnection = factory.CreateConnection(clientProvidedName:connectionName);
                }
                catch (Exception ex)
                {
                    AddFactoryInfo(ex, factory);
                    throw;
                }

                int? maxConcurrentChannels = _amqpRabbitConfigurationProvider.GetConnectionConfigData(connectionName).MaxConcurrentChannels;
                if (maxConcurrentChannels.HasValue)
                {
                    _connections[connectionName] = new RabbitConnectionPooledChannels(newRabbitMqConnection, maxConcurrentChannels.Value);
                }
                else
                {
                    _connections[connectionName] =
                        new RabbitConnection(newRabbitMqConnection);
                }

                _serverConfigurator.InitializeConfiguredObjects(_connections[connectionName], connectionName);
            }
            return _connections[connectionName];
        }

        private static void AddFactoryInfo(Exception exception, RabbitMQ.Client.ConnectionFactory factory)
        {
            if (factory == null) return;

            if (factory.Endpoint != null)
            {
                exception.Data.Add("Endpoint", factory.Endpoint.ToString());
            }
            
            if (!string.IsNullOrWhiteSpace(factory.HostName))
            {
                exception.Data.Add("HostName", factory.HostName);
            }
            
            exception.Data.Add("Port", factory.Port.ToString());
            
            if (!string.IsNullOrWhiteSpace(factory.UserName))
            {
                exception.Data.Add("UserName", factory.UserName);
            }
            
            if (!string.IsNullOrWhiteSpace(factory.VirtualHost))
            {
                exception.Data.Add("VirtualHost", factory.VirtualHost);
            }
        }

        #region IDisposable
        ~RabbitConnectionFactory()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                Parallel.ForEach(_connections, item =>
                {
                    try
                    {
                        item.Value.Close(1, "object destructor called");
                        item.Value.Dispose();
                    }
                    catch (Exception)
                    {
                        //if for some reason the thread is already closed (the cause of the original exception) 
                    }
                });
            _connections = null;
            _disposed = true;
        }
        #endregion
    }
}