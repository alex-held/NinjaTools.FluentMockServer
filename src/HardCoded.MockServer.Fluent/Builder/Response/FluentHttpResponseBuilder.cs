using System;
using System.Net;
using HardCoded.MockServer.Models;
using HardCoded.MockServer.Models.HttpEntities;
using HardCoded.MockServer.Models.ValueTypes;
using Newtonsoft.Json;

namespace HardCoded.MockServer.Fluent.Builder
{
    internal class FluentHttpResponseBuilder : IFluentHttpResponseBuilder, IWithContent<IFluentHttpResponseBuilder>
    {
        private readonly HttpResponse _httpResponse;

        public FluentHttpResponseBuilder(int statusCode)
        {
            _httpResponse = new HttpResponse(statusCode);
        }

        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithBody(Action<IWithContent<IFluentHttpResponseBuilder>> contentFactory)
        {
            contentFactory(this);
            return this;
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
        public IFluentHttpResponseBuilder WithConnectionOptions(Action<IFluentConnectionOptionsBuilder> connectionOptionsFactory)
        {
            var builder = new FluentConnectionOptionsBuilder();
            connectionOptionsFactory(builder);
            _httpResponse.ConnectionOptions = builder.Build();
            return this;
        }
        
        /// <inheritdoc />
        public HttpResponse Build() =>
            _httpResponse;

        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithJsonContent(string content)
        {
            _httpResponse.Body = Body.For(Body.BodyType.JSON); 
            _httpResponse.Body.Add("json", content);
            return this;
        }

        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithXmlContent(string content) =>
            throw new NotImplementedException();

        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithBinaryContent(byte[] content) =>
            throw new NotImplementedException();

        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithJsonArray<T>(params T[] items) =>
            throw new NotImplementedException();

        /// <inheritdoc />
        public IFluentHttpResponseBuilder WithJson<T>(T item)
        {
            var json = JsonConvert.SerializeObject(item);
            return WithJsonContent(json);
        }

        /// <inheritdoc />
        public IFluentHttpResponseBuilder MatchExactJsonContent(string content) =>
            throw new NotImplementedException();
    }
}