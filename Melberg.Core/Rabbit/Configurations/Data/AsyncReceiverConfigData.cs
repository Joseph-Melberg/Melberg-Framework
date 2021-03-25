namespace Melberg.Core.Rabbit.Configurations.Data
{
    public class AsyncReceiverConfigData
    {
        public string Name {get; set;}

        public string Connection {get; set;}

        public string Queue {get; set;}

        public int MaxThreads {get; set;}

        public int Prefetch {get; set;}
    }
}