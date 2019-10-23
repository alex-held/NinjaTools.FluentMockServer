using System;
using System.Collections.Generic;

using HardCoded.MockServer.Contracts.Models.HttpEntities;
using HardCoded.MockServer.Contracts.Models.ValueTypes;
using HardCoded.MockServer.Fluent.Builder.Request;


namespace HardCoded.MockServer.Fluent.Builder.Response
{
    internal class FluentHttpResponseBuilder : IFluentHttpResponseBuilder
    {
        private readonly HttpResponse _httpResponse;

        public FluentHttpResponseBuilder(int statusCode = 200)
        {
            _httpResponse = new HttpResponse(statusCode);
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
        public IFluentHttpResponseBuilder WithConnectionOptions(Action<IFluentConnectionOptionsBuilder> connectionOptionsFactory)
        {
            var builder = new FluentConnectionOptionsBuilder();
            connectionOptionsFactory(builder);
            _httpResponse.ConnectionOptions = builder.Build();
            return this;
        }
        
        /// <inheritdoc />
        public HttpResponse Build() => _httpResponse;
        
        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithBody(Action<IFluentBodyBuilder> bodyFactory)
        {
            var builder = new FluentBodyBuilder();
            bodyFactory(builder);
            _httpResponse.Body = builder.Body;
            return this;
        }
    }
}
