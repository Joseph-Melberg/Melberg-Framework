using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace MelbergFramework.Infrastructure.Rabbit.Messages;

public abstract class StandardMessage : IStandardMessage
{
    protected StandardMessage() { }
    IDictionary<string, object> _headers = new Dictionary<string, object>();

    public IDictionary<string, object> GetHeaders() => _headers;

    public abstract string GetRoutingKey();
    protected void SetHeaderValue(string key, string value)
    {
        _headers[key] =  value;
    }
}

public static class Headers
{
    public const string CorrelationId = "coid";
    public const string Timestamp = "timestamp";
}