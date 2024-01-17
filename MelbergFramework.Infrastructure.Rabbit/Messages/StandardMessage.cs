using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace MelbergFramework.Infrastructure.Rabbit.Messages;

public abstract class StandardMessage : IStandardMessage
{
    protected StandardMessage()
    {
        SetHeaderValue(Headers.Timestamp, DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture));
        SetHeaderValue(Headers.CorrelationId, Trace.CorrelationManager.ActivityId.ToString());
    }
    IDictionary<string, object> _headers = new Dictionary<string, object>();

    public IDictionary<string, object> GetHeaders() => _headers;

    public abstract string GetRoutingKey();
    protected void SetHeaderValue(string key, string value)
    {
        if (_headers.ContainsKey(key))
        {
            if (value == null)
                _headers.Remove(key);
            else
                _headers[key] = value;
        }
        else if (value != null)
        {
            _headers.Add(key, value);
        }
    }
}

public static class Headers
{
    public const string CorrelationId = "coid";
    public const string Timestamp = "timestamp";
}