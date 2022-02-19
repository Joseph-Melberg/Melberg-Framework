using System;
using System.IO;
using System.Text;
using Melberg.Infrastructure.Rabbit.Messages;

namespace Melberg.Infrastructure.Rabbit.Translator;

public class JsonToObjectTranslator<T>: IJsonToObjectTranslator<T>
{
    public T Translate(Message message)
    {
        var encoding = Encoding.GetEncoding("Utf-8");
        string content = null;

        var ms = new MemoryStream(message.Body);
        var reader = new StreamReader(ms, encoding, false);

        try
        {
            content = reader.ReadToEnd();
        }
        finally
        {
            reader.Dispose();
            ms.Dispose();
        }

        T requestObject;
        
        try
        {
            requestObject = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
        }
        catch (Exception jsonException)
        {
            throw new Exception($"Could not deserialize requestObject {jsonException}");
        }

        return requestObject;
    }
}