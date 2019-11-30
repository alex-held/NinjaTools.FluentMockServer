using System;
using System.Reflection;

namespace NinjaTools.FluentMockServer.Annotations
{
    /// <summary>
    ///     Marks an <see cref="Assembly" />
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class WhiteListAssemblyReflectionAttribute : Attribute
    {
    }
}
