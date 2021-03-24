namespace Melberg.Infrastructure.Rabbit.Configuration
{
    public class RabbitConfigurationProvider : IRabbitConfigurationProvider
    {
        private readonly RabbitConfigurationProvider _rabbitConfig;

        public RabbitConfigurationProvider(IConfiguration config)
        {
            _rabbitConfig = config.GetSection(RabbitConfiguration.ConfigurationName).G
        }

        protected virtual string GetQueueName(string queueName) => queueName;

        public IEnumerable<ConnectionFactoryConfigData> GetConnectionConfigData()
        {
            if(_rabbitConfig?.)
        } 
    
    }
}