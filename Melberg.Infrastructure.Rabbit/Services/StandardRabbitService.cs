using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Melberg.Infrastructure.Rabbit.Services
{
    public class StandardRabbitService : IStandardRabbitService
    {
        private readonly IServiceCollection _collection;
        public async Task Run()
        {
            //Scrape data
            var QueueName = "Get from file under AsyncRecievers";



            //Setup connection
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = "life";
            factory.Password = "conway";
            factory.VirtualHost = "/";
            factory.DispatchConsumersAsync = true;
            factory.HostName = "centurionx.net";
            factory.ClientProvidedName = "app:audit component:event-consumer";

            //
            IConnection connection = factory.CreateConnection();



            var channel = connection.CreateModel();
            //foreach ...
            channel.ExchangeDeclare("Inter", ExchangeType.Direct, true);
            channel.QueueDeclare(QueueName, false, false, false, null);
            channel.QueueBind(QueueName, "Inter", "/life", null);
            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (ch, ea) =>
            {

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await ConsumerMessageAsync(message);

                channel.BasicAck(ea.DeliveryTag, false);
                await Task.Yield();

            };
            var consumerTag = channel.BasicConsume(QueueName, false, consumer);
            await Task.Delay(Timeout.Infinite); 
            
        }
    }
}