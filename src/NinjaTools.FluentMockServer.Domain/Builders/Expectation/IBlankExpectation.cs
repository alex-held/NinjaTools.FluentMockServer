using System;
using System.ComponentModel;
using System.Net.Http;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Domain.Builders.Request;
using NinjaTools.FluentMockServer.Domain.FluentInterfaces;

namespace NinjaTools.FluentMockServer.Domain.Builders.Expectation
{
    /// <summary>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IBlankExpectation : IFluentInterface
    {
        IBlankExpectation WithBaseUrl(string url);
        IWithRequest OnHandling(HttpMethod method = null, [CanBeNull] Action<IFluentHttpRequestBuilder> requestFactory = null);
        IWithRequest OnHandlingAny([CanBeNull] HttpMethod method = null);
    }
}
