using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.FluentAPI.Builders.HttpEntities;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace NinjaTools.FluentMockServer.FluentAPI.Builders
{
       internal sealed class FluentExpectationBuilder : IFluentExpectationBuilder
        {
            [NotNull]
            public static Models.Expectation Create(
                [CanBeNull] HttpRequest httpRequest = null,
                [CanBeNull] HttpResponse httpResponse = null,
                [CanBeNull] HttpTemplate httpResponseTemplate = null,
                [CanBeNull] HttpForward httpForward = null,
                [CanBeNull] HttpTemplate httpForwardTemplate = null,
                [CanBeNull] HttpError httpError = null,
                [CanBeNull] Times times = null,
                [CanBeNull] LifeTime timeToLive = null) => new Models.Expectation(httpRequest, httpResponse, httpResponseTemplate, httpForward, httpResponseTemplate, httpError, times, timeToLive);
        
            [CanBeNull] private HttpRequest HttpRequest { get; set; }
            [CanBeNull] private HttpResponse HttpResponse { get; set; }
            [CanBeNull] private HttpTemplate HttpResponseTemplate { get; set; }
            [CanBeNull] private HttpForward HttpForward { get; set; }
            [CanBeNull] private HttpTemplate HttpForwardTemplate { get; set; }
            [CanBeNull] private HttpError HttpError { get; set; }
            [CanBeNull] private Times Times { get; set; }
            [CanBeNull] private LifeTime TimeToLive { get; set; }
            
            private readonly MockServerSetup _setup;
            
            public FluentExpectationBuilder() : this(new MockServerSetup())
            {
            }

            internal FluentExpectationBuilder(MockServerSetup setup)
            {
                _setup = setup;
            }

            /// <inheritdoc />
            [NotNull]
            public IBlankExpectation WithBaseUrl(string url)
            {
                _setup.BaseUrl = url;
                return this;
            }

            /// <inheritdoc />
            [NotNull]
            public IWithRequest OnHandling([CanBeNull] HttpMethod method = null, Action<IFluentHttpRequestBuilder> requestFactory = null)
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
            [NotNull]
            public IWithResponse RespondWith(int statusCode, [CanBeNull] Action<IFluentHttpResponseBuilder> responseFactory)
            {
                var builder = new FluentHttpResponseBuilder();
                responseFactory?.Invoke(builder);
                builder.WithStatusCode(statusCode);
                HttpResponse = builder.Build();
                return this;
            }

            /// <inheritdoc />
            [NotNull]
            public IWithResponse RespondWith(HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory)
            {
                return RespondWith((int) statusCode, responseFactory);
            }


            /// <inheritdoc />
            [NotNull]
            public IWithResponse RespondOnce(int statusCode, Action<IFluentHttpResponseBuilder> responseFactory)
            {
                Times = Times.Once;
                return RespondWith(statusCode, responseFactory);
            }


            /// <inheritdoc />
            [NotNull]
            public IWithResponse RespondOnce(HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory)
            {
                return RespondOnce((int) statusCode, responseFactory);
            }

            /// <inheritdoc />
            [NotNull]
            public IWithResponse RespondTimes(int times, int statusCode, [CanBeNull] Action<IFluentHttpResponseBuilder> responseFactory = null)
            {
                Times = new Times(times);
                var builder = new FluentHttpResponseBuilder();
                responseFactory?.Invoke(builder);
                builder.WithStatusCode(statusCode);
                HttpResponse = builder.Build();
                return this;
            }
            
            [NotNull]
            public IWithResponse RespondTimes([NotNull] Expression<Func<Times>> times, int statusCode, [CanBeNull] Action<IFluentHttpResponseBuilder> responseFactory = null)
            {
                Times = times.Compile().Invoke();
                var builder = new FluentHttpResponseBuilder();
                responseFactory?.Invoke(builder);
                builder.WithStatusCode(statusCode);
                HttpResponse = builder.Build();
                return this;
            }

            /// <inheritdoc />
            [NotNull]
            public IWithResponse WhichIsValidFor(int value, TimeUnit timeUnit = TimeUnit.Seconds)
            {
                TimeToLive = new LifeTime(value, timeUnit);
                return this;
            }

            /// <inheritdoc />
            [NotNull]
            public MockServerSetup Setup()
            {
                _setup.Expectations.Add(BuildExpectation());
                return _setup;
            }

            [NotNull]
            public Models.Expectation BuildExpectation()
            {
                return new Models.Expectation(HttpRequest, HttpResponse, HttpResponseTemplate, HttpForward, HttpForwardTemplate, HttpError, Times, TimeToLive);
            }


            /// <inheritdoc />
            [NotNull]
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
