using Melberg.Core.Rabbit.Configurations;
using Melberg.Infrastructure.Rabbit.Messages;
using Melberg.Infrastructure.Rabbit.Translator;

namespace Melberg.Infrastructure.Rabbit.Publishers;

public class StandardPublisher<T> : BasePublisher<T>, IStandardPublisher<T> where T : IStandardMessage
{
    private readonly IObjectToJsonTranslator _translator = new ObjectToJsonTranslator();

    public StandardPublisher(IRabbitConfigurationProvider configurationProvider): base(configurationProvider) { }
    public virtual void Send(T message)
    {
        var result = _translator.Translate(message);
        Emit(result);
    }
}