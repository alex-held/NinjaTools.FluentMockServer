using System.ComponentModel;
using HardCoded.MockServer.Contracts.FluentInterfaces;
using HardCoded.MockServer.Contracts.Models.ValueTypes;

using JetBrains.Annotations;


namespace HardCoded.MockServer.Fluent.Builder.Expectation
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IWithResponse : IFluentInterface
    {
        IWithResponse WhichIsValidFor(int value, [NotNull] TimeUnit timeUnit = TimeUnit.SECONDS);
        MockServerSetup Setup();
        IFluentExpectationBuilder And { get; }
    }
}
