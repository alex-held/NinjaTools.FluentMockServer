using System;
using System.ComponentModel;
using System.Net.Http;

using HardCoded.MockServer.Builder.Request;
using HardCoded.MockServer.Contracts.Attributes;
using HardCoded.MockServer.Contracts.FluentInterfaces;


namespace HardCoded.MockServer.Builder.Expectation
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IBlankExpectation : IFluentInterface
    {
        IWithRequest OnHandling([NotNull] HttpMethod method, [CanBeNull] Action<IFluentHttpRequestBuilder> requestFactory = null);
        IWithRequest OnHandlingAny([CanBeNull] HttpMethod method = null);
    }
}
