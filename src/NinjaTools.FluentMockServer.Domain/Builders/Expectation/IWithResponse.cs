using System.ComponentModel;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Domain.FluentInterfaces;
using NinjaTools.FluentMockServer.Domain.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Domain.Builders.Expectation
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
