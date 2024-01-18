using MelbergFramework.Core.Rabbit.Configurations;
using MelbergFramework.Infrastructure.Rabbit.Messages;
using MelbergFramework.Infrastructure.Rabbit.Translator;

namespace MelbergFramework.Infrastructure.Rabbit.Publishers;

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