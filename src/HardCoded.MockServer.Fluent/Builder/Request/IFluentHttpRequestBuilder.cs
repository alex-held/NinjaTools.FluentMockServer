using System;
using HardCoded.MockServer.Contracts.FluentInterfaces;
using HardCoded.MockServer.Contracts.Models.HttpEntities;


namespace HardCoded.MockServer.Fluent.Builder.Request
{
    public interface IFluentHttpRequestBuilder : IFluentBuilder<HttpRequest>, IFluentInterface
    {
        IFluentHttpRequestBuilder WithPath(string path);
        IFluentHttpRequestBuilder KeepConnectionAlive(bool keepalive = true);
        IFluentHttpRequestBuilder EnableEncryption(bool useSsl = true);
        IFluentHttpRequestBuilder WithContent(Action<IFluentBodyBuilder> contentFactory);
        HttpRequest Build();
    }
}