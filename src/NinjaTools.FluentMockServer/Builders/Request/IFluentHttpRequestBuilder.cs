using System;

using NinjaTools.FluentMockServer.FluentInterfaces;
using NinjaTools.FluentMockServer.Models.HttpEntities;


namespace NinjaTools.FluentMockServer.Builders
{
    public interface IFluentHttpRequestBuilder : IFluentBuilder<HttpRequest>, IFluentInterface
    {
        IFluentHttpRequestBuilder WithPath(string path);
        IFluentHttpRequestBuilder KeepConnectionAlive(bool keepalive = true);
        IFluentHttpRequestBuilder EnableEncryption(bool useSsl = true);
        IFluentHttpRequestBuilder WithContent(Action<IFluentBodyBuilder> contentFactory);
        IFluentHttpRequestBuilder WithHeaders(Action<IFluentHeaderBuilder> headerFactory);
        HttpRequest Build();
    }
}
