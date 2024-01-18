using System;
using System.Globalization;
using MelbergFramework.Infrastructure.Rabbit.Messages;

namespace MelbergFramework.Infrastructure.Rabbit.Extensions;
public static class MessageExtensions
{
    public static DateTime GetTimestamp(this Message message )
    {
        if(message.Headers.TryGetValue(Headers.Timestamp,out var timestamp) && timestamp is string)
        {
            try
            {
                return DateTime.ParseExact((string)timestamp,"o", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal); 
            }
            catch (Exception) { }
        }
        return DateTime.UtcNow;
    }
    
    public static Guid GetCoID(this Message message)
    {
        if(message.Headers.TryGetValue(Headers.CorrelationId, out var coid) && coid is string)
        {
            try
            {
                return Guid.Parse((string)coid);
            }
            catch(Exception){}
        }
        
        return Guid.NewGuid();
    }
}