using System;
namespace MelbergFramework.Common.Exceptions;

public abstract class BaseException : Exception
{
    private readonly string _message;
    private readonly Exception _exception;

    public BaseException(string message)
    {
        _message = message;
    }

    public BaseException(string message, Exception ex)
    {
        _message = message;
        _exception = ex;
    }

    public override string ToString()
    {
        var preamble = $"Exception of type {this.GetType} was thrown with this message: {this._message}";
        var inExeption = (_exception != null) ? $"Internal Error: {_exception.ToString()}":"";
        return preamble+"\n"+inExeption;
    }
}