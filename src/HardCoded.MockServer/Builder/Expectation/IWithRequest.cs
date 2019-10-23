using System;
using System.ComponentModel;
using System.Net;

using HardCoded.MockServer.Builder.Response;
using HardCoded.MockServer.Contracts.FluentInterfaces;


namespace HardCoded.MockServer.Builder.Expectation
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IWithRequest: IFluentInterface
    {
        IWithResponse RespondWith(int statusCode, Action<IFluentHttpResponseBuilder> responseFactory = null);
        IWithResponse RespondWith(HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory = null);
        
        IWithResponse RespondOnce(int statusCode, Action<IFluentHttpResponseBuilder> responseFactory = null);
        IWithResponse RespondOnce(HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory = null);
    }
}
