using System;
using System.Net;
using NinjaTools.FluentMockServer.Builders.Request;
using NinjaTools.FluentMockServer.FluentInterfaces;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;
using NinjaTools.FluentMockServer.Domain.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Builders.Response
{
    /// <summary>
    /// </summary>
    public interface IFluentHttpResponseBuilder : IFluentInterface
    {
        IFluentHttpResponseBuilder WithBody<T>(T payload) where T : class;
        IFluentHttpResponseBuilder WithBody(string value);
        IFluentHttpResponseBuilder WithBody(byte[] bytes, string contentType);
        IFluentHttpResponseBuilder FileBody(byte[] bytes, string filename, string contentType);


        IFluentHttpResponseBuilder WithStatusCode(int code);
        IFluentHttpResponseBuilder WithStatusCode(HttpStatusCode code);


        IFluentHttpResponseBuilder WithHeader(string name, string value);
        IFluentHttpResponseBuilder AddContentType(string contentType);

        IFluentHttpResponseBuilder ConfigureHeaders(Action<IFluentResponseHeaderBuilder> headerFactory);
        IFluentHttpResponseBuilder ConfigureConnection(Action<IFluentConnectionOptionsBuilder> connectionOptionsFactory);

        IFluentHttpResponseBuilder WithDelay(int value, TimeUnit timeUnit);

        HttpResponse Build();
    }
}
