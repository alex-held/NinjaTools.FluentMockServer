using System;
using System.ComponentModel;
using System.Net;
using NinjaTools.FluentMockServer.Domain.Builders.Response;
using NinjaTools.FluentMockServer.Domain.FluentInterfaces;

namespace NinjaTools.FluentMockServer.Domain.Builders.Expectation
{
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
        IWithResponse RespondTimes(int times, HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory = null);
    }
}
