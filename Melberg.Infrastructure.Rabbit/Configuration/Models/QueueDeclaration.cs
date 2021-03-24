namespace Melberg.Infrastructure.Rabbit.Configuration.Models
{
    public class QueueDeclaration
    {
        public string Name {get; set;}

        public string Connection {get; set;}

        public bool Durable {get; set;} 

        public bool Exclusive {get; set;} 

        public bool AutoDelete {get; set;}    

        public int MessageTtl {get; set;}    

        public string DeadLetterExchange {get; set;}
    }    
}