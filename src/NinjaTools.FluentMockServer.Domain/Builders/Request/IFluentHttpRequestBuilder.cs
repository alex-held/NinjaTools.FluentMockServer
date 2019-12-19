using System;
using System.Net.Http;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Domain.FluentInterfaces;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;
using static NinjaTools.FluentMockServer.Domain.Models.ValueTypes.Body;

namespace NinjaTools.FluentMockServer.Domain.Builders.Request
{
    public interface IFluentHttpRequestBuilder : IFluentBuilder<HttpRequest>
    {
        [NotNull] IFluentHttpRequestBuilder WithMethod([NotNull] HttpMethod method);
        IFluentHttpRequestBuilder WithMethod([NotNull] string method);
        IFluentHttpRequestBuilder WithPath(string path);
        IFluentHttpRequestBuilder KeepConnectionAlive(bool keepalive = true);
        IFluentHttpRequestBuilder EnableEncryption(bool useSsl = true);
        IFluentHttpRequestBuilder WithContent(Action<IFluentBodyBuilder> contentFactory);
        IFluentHttpRequestBuilder WithBody(BodyType type, string value);
        IFluentHttpRequestBuilder ConfigureHeaders(Action<IFluentHeaderBuilder> headerFactory);
        IFluentHttpRequestBuilder AddContentType(string contentType);
    }
}
