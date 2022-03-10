using System;
namespace Melberg.Common.Exceptions;

public class ConfigurationException : BaseException
{
    public ConfigurationException(string message) : base(message) { }
}