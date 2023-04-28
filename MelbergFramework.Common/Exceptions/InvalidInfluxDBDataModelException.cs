using System;
namespace MelbergFramework.Common.Exceptions;

public class InvalidInfluxDBDataModelException : BaseException
{
    public InvalidInfluxDBDataModelException(string message) : base(message) { }

    public InvalidInfluxDBDataModelException(string message, Exception ex) : base(message, ex) { }
}