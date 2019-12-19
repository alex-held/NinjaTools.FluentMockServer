using System;
using System.Collections.Generic;
using System.Net.Http;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;
using static NinjaTools.FluentMockServer.Domain.Models.ValueTypes.Body;

namespace NinjaTools.FluentMockServer.Domain.Builders.Request
{
    public sealed class FluentHttpRequestBuilder : IFluentHttpRequestBuilder
    {
        [CanBeNull] private  string Method;
        [CanBeNull] private  Dictionary<string, string[]> Headers;
        [CanBeNull] private  Dictionary<string, string> Cookies;
        [CanBeNull] private  JToken Body;
        [CanBeNull] private  string Path;
        private  bool? Secure;
        private  bool? KeepAlive;

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpRequestBuilder WithMethod(HttpMethod method)
        {
            return WithMethod(method.Method);
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpRequestBuilder WithMethod(string method)
        {
            Method = method;
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpRequestBuilder WithPath([CanBeNull] string path)
        {
            Path = path?.TrimStart('/').Insert(0, "/") ?? "/";
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpRequestBuilder KeepConnectionAlive(bool keepalive = true)
        {
            KeepAlive = keepalive;
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpRequestBuilder EnableEncryption(bool useSsl = true)
        {
            Secure = true;
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpRequestBuilder WithBody(BodyType type, string value)
        {
            var builder = new FluentBodyBuilder();

            switch (type)
            {
                case BodyType.JSON:
                    builder.WithExactJson(value);
                    break;
                case BodyType.JSON_PATH:
                    builder.MatchingJsonPath(value);
                    break;
                case BodyType.JSON_SCHEMA:
                    builder.MatchingJsonSchema(value);
                    break;
                case BodyType.XML:
                    builder.WithXmlContent(value);
                    break;
                case BodyType.XPATH:
                    builder.MatchingXPath(value);
                    break;
                case BodyType.XML_SCHEMA:
                    builder.MatchingXmlSchema(value);
                    break;
                case BodyType.STRING:
                    builder.WithExactContent(value);
                    break;
            }

            Body = builder.Build();
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpRequestBuilder ConfigureHeaders([CanBeNull] Action<IFluentHeaderBuilder> headerFactory)
        {
            var builder = new FluentHeaderBuilder(Headers);
            headerFactory?.Invoke(builder);
            Headers = builder.Build();
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpRequestBuilder WithContent([NotNull] Action<IFluentBodyBuilder> contentFactory)
        {
            var builder = new FluentBodyBuilder();
            contentFactory(builder);
            Body = builder.Build();
            return this;
        }


        /// <inheritdoc />
        [NotNull]
        public HttpRequest Build()
        {
            return new HttpRequest(Method, Headers, Cookies, Body, Path, Secure, KeepAlive);
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpRequestBuilder AddContentType(string contentType)
        {
            Headers ??= new Dictionary<string, string[]>();
            Headers[Builders.Headers.ContentType] = new[] {contentType};
            return this;
        }
    }
}
