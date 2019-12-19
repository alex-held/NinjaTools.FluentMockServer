using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.FluentAPI
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
    
    /// <summary>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentExpectationBuilder : IBlankExpectation , IWithRequest, IWithResponse
    {
      
    }
    /// <summary>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IWithResponse : IFluentInterface
    {
        IFluentExpectationBuilder And { get; }
        IWithResponse WhichIsValidFor(int value, TimeUnit timeUnit = TimeUnit.Seconds);
        MockServerSetup Setup();
    }
        
    /// <summary>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IWithRequest : IFluentInterface
    {
        IWithResponse RespondWith(int statusCode, Action<IFluentHttpResponseBuilder> responseFactory = null);
        IWithResponse RespondWith(HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory = null);

        IWithResponse RespondOnce(int statusCode, Action<IFluentHttpResponseBuilder> responseFactory = null);
        IWithResponse RespondOnce(HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory = null);

        IWithResponse RespondTimes(int times, int statusCode, Action<IFluentHttpResponseBuilder> responseFactory = null);
    }
}
