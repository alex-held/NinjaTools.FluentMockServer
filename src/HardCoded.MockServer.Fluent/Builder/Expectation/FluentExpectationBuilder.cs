using System;
using System.Net;
using System.Net.Http;
using HardCoded.MockServer.Fluent.Builder.Request;
using HardCoded.MockServer.Fluent.Builder.Response;

using JetBrains.Annotations;


namespace HardCoded.MockServer.Fluent.Builder.Expectation
{
    internal sealed class FluentExpectationBuilder : IFluentExpectationBuilder
    {
        private readonly MockServerSetup _setup;
        private Contracts.Models.Expectation _expectation;

        private IFluentExpectationBuilder _and;


        internal FluentExpectationBuilder(MockServerSetup setup)
        {
            _expectation = new Contracts.Models.Expectation();
            _setup = setup;
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
           _setup.Expectations.Add(_expectation);

           return _setup;
        }


        /// <inheritdoc />
        IFluentExpectationBuilder IWithResponse.And
        {
            get
            {
                Setup();
                return new FluentExpectationBuilder(_setup);
            }
        }
    }
}
