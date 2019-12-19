using System;
using System.Collections.Generic;
using System.Net;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Domain.Builders.Request;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;
using NinjaTools.FluentMockServer.Domain.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Domain.Builders.Response
{
    /// <inheritdoc  cref="IFluentHttpResponseBuilder"/>
    internal class FluentHttpResponseBuilder : HttpResponse, IFluentHttpResponseBuilder
    {
        private int? StatusCode { get; set; }
        [CanBeNull] private Delay Delay { get; set; }
        [CanBeNull] private ConnectionOptions ConnectionOptions { get; set; }
        [CanBeNull] private JToken Body { get; set; }
        [CanBeNull] private Dictionary<string, string[]> Headers { get; set; }

        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithStatusCode(HttpStatusCode statusCode)
        {
            return WithStatusCode((int) statusCode);
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpResponseBuilder WithHeader([NotNull] string name, string value)
        {
            Headers ??= new Dictionary<string, string[]>();
            Headers[name] = new[] {value};
            return this;
        }


        /// <inheritdoc />
        [NotNull]
        public IFluentHttpResponseBuilder WithStatusCode(int statusCode)
        {
            StatusCode = statusCode;
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpResponseBuilder WithDelay(int value, TimeUnit timeUnit)
        {
            Delay = new Delay(timeUnit, value);
            return this;
        }


        /// <inheritdoc />
        public IFluentHttpResponseBuilder FileBody(byte[] bytes, string filename, string contentType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpResponseBuilder ConfigureHeaders([CanBeNull] Action<IFluentResponseHeaderBuilder> headerFactory)
        {
            var builder = new FluentHeaderBuilder(Headers);
            headerFactory?.Invoke(builder);
            Headers = builder.Build();
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpResponseBuilder ConfigureConnection([NotNull] Action<IFluentConnectionOptionsBuilder> connectionOptionsFactory)
        {
            var builder = new ConnectionOptions.FluentConnectionOptionsBuilder();
            connectionOptionsFactory(builder);
            ConnectionOptions = builder.Build();
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpResponseBuilder AddContentType(string contentType)
        {
            Headers ??= new Dictionary<string, string[]>();
            Headers[Builders.Headers.ContentType] = new[] {contentType};
            return this;
        }


        /// <inheritdoc />
        public HttpResponse Build()
        {
            return new HttpResponse(StatusCode, Delay, ConnectionOptions, Body, Headers);
        }

        public IFluentHttpResponseBuilder WithBody(byte[] bytes, string contentType = null)
        {
            return WithBinaryBody(Convert.ToBase64String(bytes), contentType);
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentHttpResponseBuilder WithBody<T>(T payload) where T : class
        {
            var json = JsonConvert.SerializeObject(payload, Formatting.Indented);
            Body = new JValue(json);
            AddContentType("application/json");
            return this;
        }

        [NotNull]
        public IFluentHttpResponseBuilder WithBody(string bodyLiteral)
        {
            Body = new JValue(bodyLiteral);
            return this;
        }

        [NotNull]
        private IFluentHttpResponseBuilder WithBinaryBody(string base64, string contentType)
        {
            Body = new BinaryContent(base64);
            AddContentType(contentType);
            return this;
        }

        [NotNull]
        public IFluentHttpResponseBuilder WithBinaryFileBody(byte[] bytes, string filename, string name, string contentType)
        {
            var base64 = Convert.ToBase64String(bytes);
            WithContentDispositionHeader("form-data", name, filename);
            return WithBinaryBody(base64, contentType);
        }


        /// <inheritdoc />
        [NotNull]
        public IFluentHttpResponseBuilder WithContentDispositionHeader(string type, string name, string filename)
        {
            var builder = new FluentHeaderBuilder(Headers);
            builder.WithContentDispositionHeader(type, name, filename);
            Headers = builder.Build();
            return this;
        }
    }
}
