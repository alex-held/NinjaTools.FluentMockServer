using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Builders.Request;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Builders.Response
{
    /// <inheritdoc />
    internal class FluentHttpResponseBuilder : IFluentHttpResponseBuilder
    {
        private readonly HttpResponse _httpResponse;

        public FluentHttpResponseBuilder()
        {
            _httpResponse = new HttpResponse();
        }

        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithStatusCode(HttpStatusCode statusCode)
        {
            return WithStatusCode((int) statusCode);
        }

        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithHeader(string name, string value)
        {
            _httpResponse.Headers ??= new Dictionary<string, string[]>();
            _httpResponse.Headers[name] = new[] {value};
            return this;
        }


        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithStatusCode(int statusCode)
        {
            _httpResponse.StatusCode = statusCode;
            return this;
        }

        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithDelay(int value, TimeUnit timeUnit)
        {
            _httpResponse.Delay = new Delay
            {
                Value = value,
                TimeUnit = timeUnit
            };
            return this;
        }


        /// <inheritdoc />
        public IFluentHttpResponseBuilder FileBody(byte[] bytes, string filename, string contentType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IFluentHttpResponseBuilder ConfigureHeaders(Action<IFluentResponseHeaderBuilder> headerFactory)
        {
            var builder = new FluentHeaderBuilder(_httpResponse.Headers);
            headerFactory?.Invoke(builder);
            _httpResponse.Headers = builder.Build();
            return this;
        }

        /// <inheritdoc />
        public IFluentHttpResponseBuilder ConfigureConnection(Action<IFluentConnectionOptionsBuilder> connectionOptionsFactory)
        {
            var builder = new FluentConnectionOptionsBuilder();
            connectionOptionsFactory(builder);
            _httpResponse.ConnectionOptions = builder.Build();
            return this;
        }

        /// <inheritdoc />
        public IFluentHttpResponseBuilder AddContentType(string contentType)
        {
            _httpResponse.Headers ??= new Dictionary<string, string[]>();
            _httpResponse.Headers[Headers.ContentType] = new[] {contentType};
            return this;
        }


        /// <inheritdoc />
        public HttpResponse Build()
        {
            return _httpResponse;
        }

        public IFluentHttpResponseBuilder WithBody(byte[] bytes, string contentType = null)
        {
            return WithBinaryBody(Convert.ToBase64String(bytes), contentType);
        }

        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithBody<T>(T payload) where T : class
        {
            var json = JsonConvert.SerializeObject(payload, Formatting.Indented);
            _httpResponse.Body = new JValue(json);
            AddContentType("application/json");
            return this;
        }

        public IFluentHttpResponseBuilder WithBody(string bodyLiteral)
        {
            _httpResponse.Body = new JValue(bodyLiteral);
            return this;
        }

        private IFluentHttpResponseBuilder WithBinaryBody(string base64, string contentType)
        {
            _httpResponse.Body = new BinaryContent(base64);
            AddContentType(contentType);
            return this;
        }

        public IFluentHttpResponseBuilder WithBinaryFileBody(byte[] bytes, string filename, string name, string contentType)
        {
            var base64 = Convert.ToBase64String(bytes);
            WithContentDispositionHeader("form-data", name, filename);
            return WithBinaryBody(base64, contentType);
        }


        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithContentDispositionHeader(string type, string name, string filename)
        {
            var builder = new FluentHeaderBuilder(_httpResponse.Headers);
            builder.WithContentDispositionHeader(type, name, filename);
            _httpResponse.Headers = builder.Build();
            return this;
        }
    }
}
