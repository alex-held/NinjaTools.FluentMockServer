using System;
using System.Net;
using System.Net.Http;
using JetBrains.Annotations;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.Domain.Builders.Expectation;
using NinjaTools.FluentMockServer.Domain.Builders.Request;
using NinjaTools.FluentMockServer.Domain.Builders.Response;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;
using NinjaTools.FluentMockServer.Domain.Models.ValueTypes;
using NinjaTools.FluentMockServer.Domain.Serialization;

namespace NinjaTools.FluentMockServer.Domain.Models
{
    /// <summary>
    ///     Model to set up an Expectation on the MockServer.
    /// </summary>
    [JsonConverter(typeof(ExpectationConverter))]
    public partial class Expectation
    {
        private Expectation()
        {
        }
        
        [JsonConstructor]
        public Expectation(
            [CanBeNull] HttpRequest httpRequest,
            [CanBeNull] HttpResponse httpResponse,
            [CanBeNull] HttpTemplate httpResponseTemplate, 
            [CanBeNull] HttpForward httpForward,
            [CanBeNull] HttpTemplate httpForwardTemplate,
            [CanBeNull] HttpError httpError,
            [CanBeNull] Times times,
            [CanBeNull] LifeTime timeToLive)
        {
            HttpRequest = httpRequest;
            HttpResponse = httpResponse;
            HttpResponseTemplate = httpResponseTemplate;
            HttpForward = httpForward;
            HttpForwardTemplate = httpForwardTemplate;
            HttpError = httpError;
            Times = times;
            TimeToLive = timeToLive;
        }

        /// <summary>
        ///     The <see cref="HttpRequest" /> to match.
        /// </summary>
        public HttpRequest HttpRequest { get; private set; }

        /// <summary>
        ///     The <see cref="HttpResponse" /> to respond with.
        /// </summary>
        public HttpResponse HttpResponse { get; private set;}

        public HttpTemplate HttpResponseTemplate { get; private set;}

        /// <summary>
        ///     The Target specification to forward the matched <see cref="HttpRequest" /> to.
        /// </summary>
        public HttpForward HttpForward { get; private set;}

        public HttpTemplate HttpForwardTemplate { get; private set;}

        /// <summary>
        ///     An <see cref="HttpError" /> to respond with in case the <see cref="HttpRequest" /> has been matched.
        /// </summary>
        public HttpError HttpError { get;private set; }

        /// <summary>
        ///     How many times the MockServer should expect this setup.
        /// </summary>
        public Times Times { get;private set; }

        /// <summary>
        ///     How long the MockServer should expect this setup.
        /// </summary>
        public LifeTime TimeToLive { get; private set;}

        
        internal sealed class FluentExpectationBuilder : IFluentExpectationBuilder
        {
            public static Expectation Create(
                [CanBeNull] HttpRequest httpRequest = null,
                [CanBeNull] HttpResponse httpResponse = null,
                [CanBeNull] HttpTemplate httpResponseTemplate = null,
                [CanBeNull] HttpForward httpForward = null,
                [CanBeNull] HttpTemplate httpForwardTemplate = null,
                [CanBeNull] HttpError httpError = null,
                [CanBeNull] Times times = null,
                [CanBeNull] LifeTime timeToLive = null) => new Expectation(httpRequest, httpResponse, httpResponseTemplate, httpForward, httpResponseTemplate, httpError, times, timeToLive);
        
            [CanBeNull] private HttpRequest HttpRequest { get; set; }
            [CanBeNull] private HttpResponse HttpResponse { get; set; }
            [CanBeNull] private HttpTemplate HttpResponseTemplate { get; set; }
            [CanBeNull] private HttpForward HttpForward { get; set; }
            [CanBeNull] private HttpTemplate HttpForwardTemplate { get; set; }
            [CanBeNull] private HttpError HttpError { get; set; }
            [CanBeNull] private Times Times { get; set; }
            [CanBeNull] private LifeTime TimeToLive { get; set; }


            private readonly MockServerSetup _setup;
            private IFluentExpectationBuilder _and;


            public FluentExpectationBuilder() : this(new MockServerSetup())
            {
            }

            internal FluentExpectationBuilder(MockServerSetup setup)
            {
                _setup = setup;
            }

            /// <inheritdoc />
            public IBlankExpectation WithBaseUrl(string url)
            {
                _setup.BaseUrl = url;
                return this;
            }

            /// <inheritdoc />
            public IWithRequest OnHandling(HttpMethod method = null, Action<IFluentHttpRequestBuilder> requestFactory = null)
            {
                var builder = new FluentHttpRequestBuilder();
                requestFactory?.Invoke(builder);
                builder.WithMethod(method);
                HttpRequest = builder.Build();
                return this;
            }


            /// <inheritdoc />
            [NotNull]
            public IWithRequest OnHandlingAny(HttpMethod method = null)
            {
                if (method is null)
                {
                    HttpRequest = null;
                    return this;
                }

                HttpRequest = new FluentHttpRequestBuilder().WithMethod(method).Build();
                return this;
            }


            /// <inheritdoc />
            public IWithResponse RespondWith(int statusCode, Action<IFluentHttpResponseBuilder> responseFactory)
            {
                var builder = new FluentHttpResponseBuilder();
                responseFactory?.Invoke(builder);
                builder.WithStatusCode(statusCode);
                HttpResponse = builder.Build();
                return this;
            }

            /// <inheritdoc />
            public IWithResponse RespondWith(HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory)
            {
                return RespondWith((int) statusCode, responseFactory);
            }


            /// <inheritdoc />
            public IWithResponse RespondOnce(int statusCode, Action<IFluentHttpResponseBuilder> responseFactory)
            {
                Times = Times.Once;
                return RespondWith(statusCode, responseFactory);
            }


            /// <inheritdoc />
            public IWithResponse RespondOnce(HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory)
            {
                return RespondOnce((int) statusCode, responseFactory);
            }

            /// <inheritdoc />
            public IWithResponse RespondTimes(int times, int statusCode, Action<IFluentHttpResponseBuilder> responseFactory = null)
            {
                Times = new Times(times);
                var builder = new FluentHttpResponseBuilder();
                responseFactory?.Invoke(builder);
                builder.WithStatusCode(statusCode);
                HttpResponse = builder.Build();
                return this;
            }

            /// <inheritdoc />
            public IWithResponse RespondTimes(int times, HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory = null)
            {
                return RespondTimes(times, (int) statusCode, responseFactory);
            }


            /// <inheritdoc />
            public IWithResponse WhichIsValidFor(int value, TimeUnit timeUnit = TimeUnit.Seconds)
            {
                TimeToLive = new LifeTime(value, timeUnit);
                return this;
            }

            /// <inheritdoc />
            public MockServerSetup Setup()
            {
                _setup.Expectations.Add(BuildExpectation());
                return _setup;
            }

            public Expectation BuildExpectation()
            {
                return new Expectation(HttpRequest, HttpResponse, HttpResponseTemplate, HttpForward, HttpForwardTemplate, HttpError, Times, TimeToLive);
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
}
