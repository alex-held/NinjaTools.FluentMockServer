using System.ComponentModel;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.FluentInterfaces;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Builders.Expectation
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IWithResponse : IFluentInterface
    {
        IWithResponse WhichIsValidFor(int value, [NotNull] TimeUnit timeUnit = TimeUnit.Seconds);
        MockServerSetup Setup();
        IFluentExpectationBuilder And { get; }
    }
}
