using System;
using System.Net.Http;
using System.Runtime.CompilerServices;

using NinjaTools.FluentMockServer.FluentInterfaces;
using NinjaTools.FluentMockServer.Models.HttpEntities;


[assembly: InternalsVisibleTo("NinjaTools.FluentMockServer.Fluent.Tests")]
namespace NinjaTools.FluentMockServer.Builders
{
    internal sealed class FluentHttpRequestBuilder : IFluentHttpRequestBuilder,  IFluentBuilder<HttpRequest>, IFluentHeaderBuilder
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
                HttpMethod = method
            };
        }

        public FluentHttpRequestBuilder()
        {
            _httpRequest = new HttpRequest();
        }

        /// <inheritdoc />
        public IFluentHttpRequestBuilder WithPath(string path)
        {
            path = path?.TrimStart('/').Insert(0, "/") ?? "/";
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
        public IFluentHttpRequestBuilder WithHeaders(Action<IFluentHeaderBuilder> headerFactory)
        {
            headerFactory(this);
            return this;
        }

        /// <inheritdoc />
        public HttpRequest Build() => _httpRequest;

        /// <inheritdoc />
        public IFluentHeaderBuilder WithHeaders(params (string name, string value)[] headers)
        {
            foreach (var (name, value) in headers)
            {
                AddHeader(name, value);
            }

            return this;
        }

        /// <inheritdoc />
        public IFluentHeaderBuilder AddHeader(string name, string value)
        {
            _httpRequest.Headers.Add(name, value);
            return this;
        }
    }
}
