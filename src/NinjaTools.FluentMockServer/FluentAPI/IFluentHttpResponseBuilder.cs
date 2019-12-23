using System;
using System.Net;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.FluentAPI
{
    /// <summary>
    /// </summary>
    public interface IFluentHttpResponseBuilder : IFluentInterface
    {
        [NotNull]
        IFluentHttpResponseBuilder WithBody<T>(T payload) where T : class;

        [NotNull]
        IFluentHttpResponseBuilder WithBody(string value);

        [NotNull]
        IFluentHttpResponseBuilder WithBody(byte[] bytes, string contentType);

        [NotNull]
        IFluentHttpResponseBuilder FileBody(byte[] bytes, string filename, string contentType);


        [NotNull]
        IFluentHttpResponseBuilder WithStatusCode(int code);

        [NotNull]
        IFluentHttpResponseBuilder WithStatusCode(HttpStatusCode code);


        [NotNull]
        IFluentHttpResponseBuilder WithHeader(string name, string value);

        [NotNull]
        IFluentHttpResponseBuilder AddContentType(string contentType);

        [NotNull]
        IFluentHttpResponseBuilder ConfigureHeaders(Action<IFluentResponseHeaderBuilder> headerFactory);

        [NotNull]
        IFluentHttpResponseBuilder ConfigureConnection(Action<IFluentConnectionOptionsBuilder> connectionOptionsFactory);

        [NotNull]
        IFluentHttpResponseBuilder WithDelay(int value, TimeUnit timeUnit);

        [NotNull]
        IFluentHttpResponseBuilder WithContentDispositionHeader(string type, string name, string filename);

        [NotNull]
        HttpResponse Build();
    }
}
