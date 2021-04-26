using System;

namespace Melberg.Infrastructure.Rabbit.Connection
{
    public interface IConnection : IDisposable
    {
        bool IsOpen { get; }

        IChannel CreateChannel();

        void ReleaseChannel(IChannel channel);

        void Close(ushort reasonCode, string reasonText);
    }
}