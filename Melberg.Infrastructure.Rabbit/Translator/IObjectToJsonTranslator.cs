using Melberg.Infrastructure.Rabbit.Messages;

namespace Melberg.Infrastructure.Rabbit.Translator;

public interface IObjectToJsonTranslator
{
    Message Translate(IStandardMessage message);
}