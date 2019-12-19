using System;
using System.Collections.Generic;
using System.Net.Http;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.FluentAPI.Builders.ValueObjects;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.FluentAPI.Builders.HttpEntities
{
    internal sealed class FluentHttpRequestBuilder : IFluentHttpRequestBuilder
    {
        [CanBeNull] private string _method;
        [CanBeNull] private Dictionary<string, string[]> _headers;
        [CanBeNull] private JToken _body;
        [CanBeNull] private string _path;
        private bool? _secure;
        private bool? _keepAlive;
        
        // TODO: Add Builder Methods
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        [CanBeNull] private Dictionary<string, string> Cookies { get; set; }

        /// <inheritdoc />
        public IFluentHttpRequestBuilder WithMethod(HttpMethod method)
        {
            return WithMethod(method.Method);
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpRequestBuilder WithMethod(string method)
        {
            _method = method;
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpRequestBuilder WithPath([CanBeNull] string path)
        {
            _path = path?.TrimStart('/').Insert(0, "/") ?? "/";
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpRequestBuilder KeepConnectionAlive(bool keepalive = true)
        {
            _keepAlive = keepalive;
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpRequestBuilder EnableEncryption(bool useSsl = true)
        {
            _secure = true;
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpRequestBuilder WithBody(Body.BodyType type, string value)
        {
            var builder = new FluentBodyBuilder();

            switch (type)
            {
                case Body.BodyType.JSON:
                    builder.WithExactJson(value);
                    break;
                case Body.BodyType.JSON_PATH:
                    builder.MatchingJsonPath(value);
                    break;
                case Body.BodyType.JSON_SCHEMA:
                    builder.MatchingJsonSchema(value);
                    break;
                case Body.BodyType.XML:
                    builder.WithXmlContent(value);
                    break;
                case Body.BodyType.XPATH:
                    builder.MatchingXPath(value);
                    break;
                case Body.BodyType.XML_SCHEMA:
                    builder.MatchingXmlSchema(value);
                    break;
                case Body.BodyType.STRING:
                    builder.WithExactContent(value);
                    break;
            }

            _body = builder.Build();
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpRequestBuilder ConfigureHeaders([CanBeNull] Action<IFluentHeaderBuilder> headerFactory)
        {
            var builder = new FluentHeaderBuilder(_headers);
            headerFactory?.Invoke(builder);
            _headers = builder.Build();
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpRequestBuilder WithContent([NotNull] Action<IFluentBodyBuilder> contentFactory)
        {
            var builder = new FluentBodyBuilder();
            contentFactory(builder);
            _body = builder.Build();
            return this;
        }


        /// <inheritdoc />
        [NotNull]
        public HttpRequest Build()
        {
            return new HttpRequest(_method, _headers, Cookies, _body, _path, _secure, _keepAlive);
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpRequestBuilder AddContentType(string contentType)
        {
            _headers ??= new Dictionary<string, string[]>();
            _headers[Utils.Headers.ContentType] = new[] {contentType};
            return this;
        }
    }
}
