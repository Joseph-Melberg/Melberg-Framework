using MelbergFramework.Infrastructure.Rabbit.Messages;

namespace MelbergFramework.Infrastructure.Rabbit.Translator;

public interface IObjectToJsonTranslator
{
    Message Translate(IStandardMessage message);
}