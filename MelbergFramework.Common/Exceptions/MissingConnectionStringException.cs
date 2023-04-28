using System;

namespace MelbergFramework.Common.Exceptions;

public class MissingConnectionStringException : BaseException
{
    public MissingConnectionStringException(string message) : base(message) { }

    public MissingConnectionStringException(string message, Exception ex) : base(message, ex) { }
}