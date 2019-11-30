using System.ComponentModel;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Client.Models.ValueTypes;
using NinjaTools.FluentMockServer.FluentInterfaces;

namespace NinjaTools.FluentMockServer.Builders.Expectation
{
    /// <summary>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IWithResponse : IFluentInterface
    {
        IFluentExpectationBuilder And { get; }
        IWithResponse WhichIsValidFor(int value, [NotNull] TimeUnit timeUnit = TimeUnit.Seconds);
        MockServerSetup Setup();
    }
}
