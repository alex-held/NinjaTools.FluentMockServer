using System;
using System.Collections.Generic;
using System.Net.Http;
using FluentApi.Generics.Framework;
using HardCoded.MockServer.Models;
using HardCoded.MockServer.Models.HttpEntities;
using Newtonsoft.Json;

namespace HardCoded.MockServer.Fluent.Builder
{
    internal sealed class FluentHttpRequestBuilder : IFluentHttpRequestBuilder
    {
        private readonly HttpRequest _httpRequest;
        
        public FluentHttpRequestBuilder(HttpMethod method)
        {
            _httpRequest = new HttpRequest
            {
                Method = method.Method, 
            };
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
            _httpRequest.Body = builder.Build();
            return this;
        }

        /// <inheritdoc />
        public HttpRequest Build() =>
            _httpRequest;
    }

    public interface IFluentHeaderBuilder : IFluentInterface
    {
        IFluentHeaderBuilder WithHeaders(params (string name, string value)[] headers);
        IFluentHeaderBuilder AddHeader(string name, string value);
    }
    
}