using System;
using NinjaTools.FluentMockServer.FluentInterfaces;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;
using static NinjaTools.FluentMockServer.Client.Models.ValueTypes.Body;

namespace NinjaTools.FluentMockServer.Builders.Request
{
    public interface IFluentHttpRequestBuilder : IFluentBuilder<HttpRequest>
    {
        IFluentHttpRequestBuilder WithPath(string path);
        IFluentHttpRequestBuilder KeepConnectionAlive(bool keepalive = true);
        IFluentHttpRequestBuilder EnableEncryption(bool useSsl = true);
        IFluentHttpRequestBuilder WithContent(Action<IFluentBodyBuilder> contentFactory);
        IFluentHttpRequestBuilder WithBody(BodyType type, string value);
        IFluentHttpRequestBuilder ConfigureHeaders(Action<IFluentHeaderBuilder> headerFactory);
        IFluentHttpRequestBuilder AddContentType(string contentType);
    }
}
