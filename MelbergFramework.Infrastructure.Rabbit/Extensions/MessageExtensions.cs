using System;
using System.Globalization;
using MelbergFramework.Infrastructure.Rabbit.Messages;

namespace MelbergFramework.Infrastructure.Rabbit.Extensions;
internal static class MessageExtensions
{
    public static DateTime GetTimestamp(this Message message )
    {
        if(message.Headers.TryGetValue(Headers.Timestamp,out var timestamp))
        {
            try
            {
                
                return DateTime.ParseExact((string)timestamp,"o", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal); 
            }
            catch (System.Exception)
            {
                
            }
        }
        return DateTime.UtcNow;
    }
}