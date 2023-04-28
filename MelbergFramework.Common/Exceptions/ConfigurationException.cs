using System;
namespace MelbergFramework.Common.Exceptions;

public class ConfigurationException : BaseException
{
    public ConfigurationException(string message) : base(message) { }
}