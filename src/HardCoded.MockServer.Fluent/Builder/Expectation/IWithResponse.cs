using System.ComponentModel;
using HardCoded.MockServer.Contracts.FluentInterfaces;

namespace HardCoded.MockServer.Fluent.Builder.Expectation
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IWithResponse : IFluentInterface
    {
        MockServerSetup Setup();
        IFluentExpectationBuilder And();
    }
}