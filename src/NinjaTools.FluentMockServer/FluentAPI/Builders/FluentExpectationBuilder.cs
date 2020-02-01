using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.FluentAPI.Builders.HttpEntities;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace NinjaTools.FluentMockServer.FluentAPI.Builders
{
     internal sealed class FluentExpectationBuilder : IFluentExpectationBuilder
        {
            [NotNull]
            public static Expectation Create(
                HttpRequest? httpRequest = null,
                HttpResponse? httpResponse = null,
                HttpError? httpError = null,
                Times? times = null,
                LifeTime? timeToLive = null,
                MockContext context = null) => new Expectation(httpRequest, httpResponse, httpError, times, timeToLive, context);

            private MockContext? Context { get; set; }
            private HttpRequest? HttpRequest { get; set; }
            private HttpResponse? HttpResponse { get; set; }
            private HttpError? HttpError { get; set; }
            private Times? Times { get; set; }
            private LifeTime? TimeToLive { get; set; }

            private readonly MockServerSetup _setup;

            public FluentExpectationBuilder() : this(new MockServerSetup())
            {
            }

            internal FluentExpectationBuilder(MockServerSetup setup)
            {
                _setup = setup;
            }

            /// <inheritdoc />
            public IBlankExpectation UsingContext(string context)
            {
                Context = new MockContext(context);
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
            public IWithResponse RespondWith(Action<IFluentHttpResponseBuilder> responseFactory)
            {
                var builder = new FluentHttpResponseBuilder();
                responseFactory.Invoke(builder);
                HttpResponse = builder.Build();
                return this;
            }

            /// <inheritdoc />
            public IWithResponse RespondWith(HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder>? responseFactory)
            {
                var builder = new FluentHttpResponseBuilder();
                responseFactory?.Invoke(builder);
                builder.WithStatusCode(statusCode);
                HttpResponse = builder.Build();
                return this;
            }

            /// <inheritdoc />
            public IWithResponse RespondOnce(Action<IFluentHttpResponseBuilder> responseFactory)
            {
                Times = Times.Once;
                return RespondWith(responseFactory);
            }


            /// <inheritdoc />
            [NotNull]
            public IWithResponse RespondOnce(HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder>? responseFactory)
            {
                Times = Times.Once;
                return RespondWith(statusCode, responseFactory);
            }

            /// <inheritdoc />
            [NotNull]
            public IWithResponse RespondTimes(int times, int statusCode, Action<IFluentHttpResponseBuilder>? responseFactory = null)
            {
                Times = new Times(times);
                var builder = new FluentHttpResponseBuilder();
                responseFactory?.Invoke(builder);
                builder.WithStatusCode(statusCode);
                HttpResponse = builder.Build();
                return this;
            }

            [NotNull]
            public IWithResponse RespondTimes(Expression<Func<Times>>? times, int statusCode, Action<IFluentHttpResponseBuilder>? responseFactory = null)
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
                if(Context != null)
                {
                    HttpRequest = Context.Apply(HttpRequest);
                }

                var expectation = new Expectation(HttpRequest, HttpResponse, HttpError, Times, TimeToLive, Context);
                _setup.Expectations.Add(expectation);
                return _setup;
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
