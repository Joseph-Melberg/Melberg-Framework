namespace Melberg.Infrastructure.Rabbit.Connection
{
    public interface IChannelFactory
    {
        IChannel GetChannel(string connectionName);

        void ReleaseChannel(IChannel channel);
    }
}