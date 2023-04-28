using System;

namespace MelbergFramework.Common.Exceptions;

public class InvalidInfluxDBFieldValueException : BaseException
{
    public InvalidInfluxDBFieldValueException(string message) : base(message) { }
    public InvalidInfluxDBFieldValueException(string message, Exception ex) : base(message, ex) { }
}