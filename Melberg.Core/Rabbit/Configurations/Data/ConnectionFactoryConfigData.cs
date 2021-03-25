namespace Melberg.Core.Rabbit.Configurations.Data
{
    public class ConnectionFactoryConfigData
    {
        public string Name {get; set;}

        public string ServerName {get; set;}

        public int? MaxConcurrentChannels {get; set;}
        
        public string UserName {get; set;}

        public string Password {get; set;} 

        public bool UseSsl {get; set;}
    }
}