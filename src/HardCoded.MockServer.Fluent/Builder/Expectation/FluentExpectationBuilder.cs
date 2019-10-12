using System;
using System.Net;
using System.Net.Http;
using HardCoded.MockServer.Fluent.Builder.Request;
using HardCoded.MockServer.Fluent.Builder.Response;

namespace HardCoded.MockServer.Fluent.Builder.Expectation
{
    internal sealed class FluentExpectationBuilder : IFluentExpectationBuilder
    {
        private readonly Func<Models.Expectation, MockServerSetup>  _setupCallback;
        private Models.Expectation _expectation;
        
        internal FluentExpectationBuilder(Func<Models.Expectation, MockServerSetup> setupCallback)
        {
            _expectation = new Models.Expectation();
            _setupCallback = setupCallback;
        }
        
        /// <inheritdoc />
        public IWithRequest OnHandling(HttpMethod method, Action<IFluentHttpRequestBuilder> requestFactory)
        {
            var builder = new FluentHttpRequestBuilder(method);
            requestFactory(builder);
            _expectation.HttpRequest = builder.Build();
            return this;
        }
        
        /// <inheritdoc />
        public IWithResponse RespondWith(int statusCode, Action<IFluentHttpResponseBuilder> responseFactory)
        {
            var builder = new FluentHttpResponseBuilder(statusCode);
            responseFactory(builder);
            _expectation.HttpResponse = builder.Build();
            return this;
        }

        /// <inheritdoc />
        public IWithResponse RespondWith(HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory)
        {
            return RespondWith((int) statusCode, responseFactory);
        }


        /// <inheritdoc />
        public MockServerSetup Setup()
        {
           return _setupCallback(_expectation);
        }

        /// <inheritdoc />
        public IFluentExpectationBuilder And()
        {
            _setupCallback(_expectation);
            _expectation = new Models.Expectation();
            return this;
        }
    }
}
