using Melberg.Infrastructure.Rabbit.Messages;

namespace Melberg.Infrastructure.Rabbit.Translator;

public interface IJsonToObjectTranslator<T>
{
    T Translate(Message message);
}