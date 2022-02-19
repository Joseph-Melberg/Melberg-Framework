using System.Text;
using Melberg.Infrastructure.Rabbit.Messages;
using Newtonsoft.Json;

namespace Melberg.Infrastructure.Rabbit.Translator;

public class ObjectToJsonTranslator : IObjectToJsonTranslator
{
    public Message Translate(IStandardMessage message)
    {
        var body = GetBodyBytes(message);
        
        var result = new Message()
        {
            Body = body,
            RoutingKey = message.GetRoutingKey(),
            Headers = message.GetHeaders()
        };
        return result;
    }
    private byte[] GetBodyBytes(object packetToSend)
    {
        var encoding = Encoding.GetEncoding("UTF-8");
        var jsonString = JsonConvert.SerializeObject(packetToSend);
        var jsonBytes = encoding.GetBytes(jsonString);
        return jsonBytes;
    }
}