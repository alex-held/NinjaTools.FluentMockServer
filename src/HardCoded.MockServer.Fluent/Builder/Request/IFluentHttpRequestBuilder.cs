using System;
using System.Net.Http;
using FluentApi.Generics.Framework;
using HardCoded.MockServer.Models.HttpEntities;

namespace HardCoded.MockServer.Fluent.Builder
{
    public interface IFluentHttpRequestBuilder : IFluentInterface
    {
        IFluentHttpRequestBuilder WithPath(string path);
        IFluentHttpRequestBuilder KeepConnectionAlive(bool keepalive = true);
        IFluentHttpRequestBuilder EnableEncryption(bool useSsl = true);
        IFluentHttpRequestBuilder WithContent(Action<IFluentBodyBuilder> contentFactory);
        HttpRequest Build();
    }
}