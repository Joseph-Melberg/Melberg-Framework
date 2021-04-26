using System;
using Melberg.Infrastructure.Rabbit.Connection;

namespace Chamberlain.Middleware.Infrastructure.Rabbit.Connection
{
    public interface IConnectionFactory : IDisposable
    {
        IConnection GetConnection(string connectionName);

        void EstablishConnections();
    }
}
