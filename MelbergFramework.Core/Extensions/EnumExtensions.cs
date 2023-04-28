using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MelbergFramework.Core.Extensions;
public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        return
            value
                .GetType()
                .GetMember(value.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description;      
    }
}