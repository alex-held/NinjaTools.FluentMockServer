using System;
using System.ComponentModel;
using System.Net;
using HardCoded.MockServer.Contracts.FluentInterfaces;
using HardCoded.MockServer.Fluent.Builder.Response;

namespace HardCoded.MockServer.Fluent.Builder.Expectation
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IWithRequest: IFluentInterface
    {
        IWithResponse RespondWith(int statusCode, Action<IFluentHttpResponseBuilder> responseFactory);
        IWithResponse RespondWith(HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory);
    }
}