namespace Melberg.Core.Rabbit.Configurations.Data
{
        public class QueueConfigData
    {
        public string Name { get; set; }

        public string Connection { get; set; }

        public bool AutoDelete { get; set; }

        public bool Durable { get; set; }

        public bool Exclusive { get; set; }

    }
}