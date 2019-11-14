using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;


namespace NinjaTools.FluentMockServer.Builders
{
    internal class FluentHttpResponseBuilder : IFluentHttpResponseBuilder, IFluentHeaderBuilder
    {
        private readonly HttpResponse _httpResponse;

        public FluentHttpResponseBuilder()
        {
            _httpResponse = new HttpResponse();
        }

        
        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithDelay(Action<IFluentDelayBuilder> delayFactory)
        {
            var delayBuilder = new FluentDelayBuilder();
            delayFactory(delayBuilder);
            _httpResponse.Delay = delayBuilder.Build();
            return this;
        }

        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithDelay(int value, TimeUnit timeUnit)
        {
            _httpResponse.Delay = new Delay()
            {
                Value = value, TimeUnit = timeUnit
            };
            return this;
        }


        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithHeader(string name, string value)
        {
            _httpResponse.Headers ??= new Dictionary<string, string[]>();
            _httpResponse.Headers.Add(name, new []{value});
            return this;
        }

        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithHeaders(Action<IFluentHeaderBuilder> headerFactory)
        {
            headerFactory(this);
            return this;
        }


        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithConnectionOptions(Action<IFluentConnectionOptionsBuilder> connectionOptionsFactory)
        {
            var builder = new FluentConnectionOptionsBuilder();
            connectionOptionsFactory(builder);
            _httpResponse.ConnectionOptions = builder.Build();
            return this;
        }


        /// <inheritdoc />
        public HttpResponse Build() => _httpResponse;
        
        public IFluentHttpResponseBuilder WithBinaryBody(string base64, string contentType)
        {
            _httpResponse.Body = new ResponseBody(true, base64);
            _httpResponse.Headers ??= new Dictionary<string, string[]>();
            _httpResponse.Headers["content-type"] = new[] {contentType};
            return this;
        }
        
        public IFluentHttpResponseBuilder WithBinaryFileBody(byte[] bytes, string filename, string contentType)
        {
            var base64 = Convert.ToBase64String(bytes);
            _httpResponse.Headers ??= new Dictionary<string, string[]>();
            _httpResponse.Headers["content-disposition"] = new[] {$"form-data; name=\"{filename}\"; filename=\"{filename}\""};
            return WithBinaryBody(base64, contentType);
        }
        
        public IFluentHttpResponseBuilder WithLiteralBody(string bodyLiteral, string contentType = null)
        {
            _httpResponse.Body = new ResponseBody(false, bodyLiteral);
            if (string.IsNullOrWhiteSpace(contentType)) return this;
            
            _httpResponse.Headers ??= new Dictionary<string, string[]>();
            _httpResponse.Headers["content-type"] = new[] {contentType};
            return this;
        }

        /// <inheritdoc />
        public IFluentHeaderBuilder WithHeaders(params (string name, string value)[] headers)
        {
            foreach (var (name, value)in headers)
            {
                AddHeader(name, value);
            }

            return this;
        }

        /// <inheritdoc />
        public IFluentHeaderBuilder AddHeader(string name, string value)
        {
           _httpResponse.Headers.Add(name, new []{value});
           return this;
        }
    }
}
