namespace Melberg.Infrastructure.Rabbit.Configuration.Models
{
    public class BindingDeclaration
    {
    public string Queue {get; set;}

    public string Connection {get; set;}

    public string Exchange {get; set;}

    public string SubsriptionKey {get; set;}
    }    
}