using System;

namespace MelbergFramework.Common.Exceptions;

public class InvalidKeyPairException : BaseException
{
    public InvalidKeyPairException(string message) : base(message)
    {
    }

    public InvalidKeyPairException(string message, Exception ex) : base(message, ex)
    {
    }
}