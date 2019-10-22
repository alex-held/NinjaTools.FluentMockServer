using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using HardCoded.MockServer.Contracts.FluentInterfaces;
using HardCoded.MockServer.Contracts.Models.HttpEntities;


[assembly: InternalsVisibleTo("HardCoded.MockServer.Fluent.Tests")]
namespace HardCoded.MockServer.Fluent.Builder.Request
{
    internal sealed class FluentHttpRequestBuilder : IFluentHttpRequestBuilder,  IFluentBuilder<HttpRequest>
    {
        private readonly HttpRequest _httpRequest;

        public IFluentHttpRequestBuilder WithBody(Action<IFluentBodyBuilder> bodyFactory)
        {
            var builder = new FluentBodyBuilder();
            bodyFactory(builder);
            _httpRequest.Body = builder.Build();
            return this;
        }
        
        public FluentHttpRequestBuilder(HttpMethod method)
        {
            _httpRequest = new HttpRequest
            {
                Method = method
            };
        }

        public FluentHttpRequestBuilder()
        {
            
        }

        /// <inheritdoc />
        public IFluentHttpRequestBuilder WithPath(string path)
        {
            _httpRequest.Path = path;
            return this;
        }

        /// <inheritdoc />
        public IFluentHttpRequestBuilder KeepConnectionAlive(bool keepalive = true)
        {
            _httpRequest.KeepAlive = keepalive;
            return this;
        }

        /// <inheritdoc />
        public IFluentHttpRequestBuilder EnableEncryption(bool useSsl = true)
        {
            _httpRequest.Secure = true;
            return this;
        }
        

        /// <inheritdoc />
        public IFluentHttpRequestBuilder WithContent(Action<IFluentBodyBuilder> contentFactory)
        {
            var builder = new FluentBodyBuilder();
            contentFactory(builder);
            _httpRequest.Body = builder.Body;
            return this;
        }

        /// <inheritdoc />
        public HttpRequest Build() => _httpRequest;
    }
}