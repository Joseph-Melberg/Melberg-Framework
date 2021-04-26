using System;
using System.Globalization;
using System.Text;
using Melberg.Infrastructure.Rabbit.Models;

namespace Melberg.Infrastructure.Rabbit.Extensions
{
        internal static class MessageExtensions
    {
        public static Guid GetActivityId(this Message message)
        {
            var activityIdHeader = GetHeaderValue(message, HeaderKeys.AvtivityId_Legacy, HeaderKeys.ActivityId);

            if (string.IsNullOrWhiteSpace(activityIdHeader))
            {
                throw new ArgumentNullException(
                    $"ActivityId type could not be determined because none of the specified headers are present: {string.Join(", ", HeaderKeys.AvtivityId_Legacy, HeaderKeys.ActivityId)}");
            }

            return Guid.Parse(activityIdHeader);
        }

        public static string GetMessageType(this Message message)
        {
            var messageTypeHeader = GetHeaderValue(message, HeaderKeys.MessageType_Legacy, HeaderKeys.MessageType);

            if (string.IsNullOrWhiteSpace(messageTypeHeader))
            {
                throw new ArgumentException(
                    $"MessageType could not be determined because none of the specified headers are present: {string.Join(", ", HeaderKeys.MessageType_Legacy, HeaderKeys.MessageType)}");
            }
            var messageTypeHeaderSegments = messageTypeHeader.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(_ => _.Trim()).ToArray();
            
            return messageTypeHeaderSegments.First();
        }

        public static DateTime GetTimestamp(this Message message)
        {
            var timestampHeaderHeader = GetHeaderValue(message, HeaderKeys.Timestamp);

            if (string.IsNullOrWhiteSpace(timestampHeaderHeader))
            {
                throw new ArgumentNullException(
                    $"Timestamp could not be determined because no header is present: {HeaderKeys.Timestamp}");
            }

            return DateTime.ParseExact(timestampHeaderHeader, "o", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        }

        private static string GetHeaderValue(Message message, params string[] headerNames)
        {
            var encoding = string.IsNullOrWhiteSpace(message.ContentEncoding) ? Encoding.UTF8 : Encoding.GetEncoding(message.ContentEncoding);

            foreach (var headerName in headerNames)
            {
                if (message.Headers.TryGetValue(headerName, out var headerValue))
                {
                    if (headerValue is byte[]) return encoding.GetString((byte[])headerValue);
                    else return headerValue.ToString();
                }
            }

            return null;
        }
    }
}