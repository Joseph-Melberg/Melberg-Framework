using System;
using System.Globalization;
using Melberg.Infrastructure.Rabbit.Messages;

namespace Melberg.Infrastructure.Rabbit.Extensions;
internal static class MessageExtensions
{
    public static DateTime GetTimestamp(this Message message )
    {
        if(message.Headers.TryGetValue(Headers.Timestamp,out var timestamp))
        {
           return DateTime.ParseExact((string)timestamp,"o", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal); 
        }
        throw new ArgumentNullException("Timestamp invalid, header missing");
    }
}