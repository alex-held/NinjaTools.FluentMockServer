using System;
using NinjaTools.FluentMockServer.Models;
using Xunit.Sdk;

namespace NinjaTools.FluentMockServer.Xunit.Attributes
{
    /// <summary>
    /// Unit tests marked with this <see cref="Attribute"/> will create <see cref="Expectation"/> inside an isolated
    /// Context.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class IsolatedMockServerSetupAttribute : BeforeAfterTestAttribute
    {
    }
}
