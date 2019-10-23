using System.ComponentModel;


namespace HardCoded.MockServer.Builder.Expectation
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentExpectationBuilder : IBlankExpectation, IWithRequest, IWithResponse
    {
    }
}