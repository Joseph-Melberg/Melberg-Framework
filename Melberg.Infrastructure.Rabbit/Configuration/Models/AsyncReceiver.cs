namespace Melberg.Infrastructure.Rabbit.Configuration.Models
{
    public class AsyncReceiver
    {
        public string Name {get; set;}

        public string Connection {get; set;}

        public string Queue {get; set;}

        public int MaxThreads {get; set;}

        public int Prefetch {get; set;}
    }    
}