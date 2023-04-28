using MelbergFramework.Infrastructure.Rabbit.Messages;

namespace MelbergFramework.Infrastructure.Rabbit.Translator;

public interface IJsonToObjectTranslator<T>
{
    T Translate(Message message);
}