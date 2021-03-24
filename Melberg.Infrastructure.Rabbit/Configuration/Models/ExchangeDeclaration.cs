namespace Melberg.Infrastructure.Rabbit.Configuration.Models
{
    public class ExchangeDeclaration
    {
        public string Name {get; set;}

        public string Connection {get; set;} 

        public string Type {get; set;}    

        public bool Durable {get; set;}

        public bool AutoDelete {get; set;}
    }    
}