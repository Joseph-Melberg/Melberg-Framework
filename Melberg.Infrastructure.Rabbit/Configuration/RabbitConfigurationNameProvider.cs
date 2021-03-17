namespace Melberg.Infrastructure.Rabbit.Configuration
{
    public class RabbitConfigurationNameProvider : IRabbitConfigurationNameProvider
    {
        public string ConfigurationName => "RabbitConfiguration";
    }
}