using Melberg.Core.Rabbit.Configurations;
using Melberg.Infrastructure.Rabbit.Messages;
using Melberg.Infrastructure.Rabbit.Translator;
using Microsoft.Extensions.Logging;

namespace Melberg.Infrastructure.Rabbit.Publishers;

public class StandardPublisher<T> : BasePublisher<T>, IStandardPublisher<T> where T : IStandardMessage
{
    private readonly IObjectToJsonTranslator _translator = new ObjectToJsonTranslator();

    public StandardPublisher(IRabbitConfigurationProvider configurationProvider, ILogger logger): base(configurationProvider, logger) { }
    public virtual void Send(T message)
    {
        var result = _translator.Translate(message);
        Emit(result);
    }
}