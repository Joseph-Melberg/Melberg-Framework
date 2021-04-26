using System;

namespace Melberg.Infrastructure.Rabbit.Connection
{
    public class RabbitConnection : IConnection
    {
        protected readonly RabbitMQ.Client.IConnection Connection;
        private bool _disposed;

        public RabbitConnection(RabbitMQ.Client.IConnection rabbitConnection)
        {
            Connection = rabbitConnection;
        }

        public bool IsOpen => Connection.IsOpen;

        public IChannel CreateChannel()
        {
            try
            {
                return CreateChannelCore(Connection);
            }
            catch (Exception ex)
            {
                AddFactoryInfo(ex, Connection);
                throw;
            }
        }

        protected virtual IChannel CreateChannelCore(RabbitMQ.Client.IConnection connection)
        {
            return new RabbitChannel(connection.CreateModel(), this);
        }

        public void ReleaseChannel(IChannel channel)
        {
            channel?.Dispose();
        }

        public void Close(ushort reasonCode, string reasonText)
        {
            Connection.Close(reasonCode, reasonText);
        }

        private static void AddFactoryInfo(Exception exception, RabbitMQ.Client.IConnection rabbitConnection)
        {
            if (rabbitConnection?.Endpoint != null)
            {
                exception.Data.Add("Endpoint", rabbitConnection.Endpoint.ToString());
            }
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                try
                {
                    Connection.Dispose();
                }
                catch
                {
                    //MP-1168: TES services must recover upon RabbitMQ failure
                    //RabbitMQ.Client.Framing.Impl.Connection.System.IDisposable.Dispose() can throw an error if the connection is down
                }
            }

            _disposed = true;
        }

        #endregion
    }
}