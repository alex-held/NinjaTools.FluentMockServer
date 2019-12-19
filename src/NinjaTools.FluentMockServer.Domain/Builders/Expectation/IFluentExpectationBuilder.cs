using System.ComponentModel;

namespace NinjaTools.FluentMockServer.Domain.Builders.Expectation
{
    /// <summary>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentExpectationBuilder : IBlankExpectation, IWithRequest, IWithResponse
    {
    }
}
