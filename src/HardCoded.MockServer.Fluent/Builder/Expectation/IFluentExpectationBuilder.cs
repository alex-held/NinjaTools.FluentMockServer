using System.ComponentModel;

namespace HardCoded.MockServer.Fluent.Builder.Expectation
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentExpectationBuilder : IBlankExpectation, IWithRequest, IWithResponse
    {
    }
}