using System;


namespace HardCoded.MockServer.Contracts.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class WhiteListAssemblyReflectionAttribute : Attribute { }
}
