using System.Collections.Generic;

namespace Melberg.Core.Rabbit.Configurations.Data
{
    public class SchedulerQueueConfigData
    {
        public string Name { get; set; }
        public List<string> RoutingKeyList { get; set; }
        public int TimeToLive { get; set; }
        public int MaxPriority { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
        public bool IsDeclared { get; set; }
        public bool SkipReordering { get; set; }
    }
}