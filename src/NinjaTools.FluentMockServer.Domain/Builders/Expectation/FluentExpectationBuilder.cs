namespace NinjaTools.FluentMockServer.Domain.Builders.Expectation
{
    // internal sealed class FluentExpectationBuilder : IFluentExpectationBuilder
    // {
    //     [CanBeNull] private HttpRequest HttpRequest { get; set; }
    //     [CanBeNull] private HttpResponse HttpResponse { get; set; }
    //     [CanBeNull] private HttpTemplate HttpResponseTemplate { get; set; }
    //     [CanBeNull] private HttpForward HttpForward { get; set; }
    //     [CanBeNull] private HttpTemplate HttpForwardTemplate { get; set; }
    //     [CanBeNull] private HttpError HttpError { get; set; }
    //     [CanBeNull] private Times Times { get; set; }
    //     [CanBeNull] private LifeTime TimeToLive { get; set; }
    //     
    //     
    //     private readonly MockServerSetup _setup;
    //     private IFluentExpectationBuilder _and;
    //     
    //     
    //     public FluentExpectationBuilder() : this(new MockServerSetup())
    //     {
    //     }
    //
    //     internal FluentExpectationBuilder(MockServerSetup setup)
    //     {
    //         _setup = setup;
    //     }
    //
    //     /// <inheritdoc />
    //     public IBlankExpectation WithBaseUrl(string url)
    //     {
    //         _setup.BaseUrl = url;
    //         return this;
    //     }
    //
    //     /// <inheritdoc />
    //     public IWithRequest OnHandling(HttpMethod method = null, Action<IFluentHttpRequestBuilder> requestFactory = null)
    //     {
    //         var builder = new FluentHttpRequestBuilder();
    //         requestFactory?.Invoke(builder);
    //         HttpRequest = builder.Build();
    //         HttpRequest.Method = method.Method;
    //         return this;
    //     }
    //
    //
    //     /// <inheritdoc />
    //     [NotNull]
    //     public IWithRequest OnHandlingAny(HttpMethod method = null)
    //     {
    //         if (method is null)
    //         {
    //             HttpRequest = null;
    //             return this;
    //         }
    //
    //         HttpRequest = new HttpRequest
    //         {
    //             Method = method.Method
    //         };
    //
    //         return this;
    //     }
    //
    //
    //     /// <inheritdoc />
    //     public IWithResponse RespondWith(int statusCode, Action<IFluentHttpResponseBuilder> responseFactory)
    //     {
    //         var builder = new FluentHttpResponseBuilder();
    //         responseFactory?.Invoke(builder);
    //         HttpResponse = builder.Build();
    //         HttpResponse.StatusCode = statusCode;
    //         return this;
    //     }
    //
    //     /// <inheritdoc />
    //     public IWithResponse RespondWith(HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory)
    //     {
    //         return RespondWith((int) statusCode, responseFactory);
    //     }
    //
    //
    //     /// <inheritdoc />
    //     public IWithResponse RespondOnce(int statusCode, Action<IFluentHttpResponseBuilder> responseFactory)
    //     {
    //         Times = Times.Once;
    //         return RespondWith(statusCode, responseFactory);
    //     }
    //
    //
    //     /// <inheritdoc />
    //     public IWithResponse RespondOnce(HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory)
    //     {
    //         return RespondOnce((int) statusCode, responseFactory);
    //     }
    //
    //     /// <inheritdoc />
    //     public IWithResponse RespondTimes(int times, int statusCode, Action<IFluentHttpResponseBuilder> responseFactory = null)
    //     {
    //         Times = new Times(times);
    //         var builder = new FluentHttpResponseBuilder();
    //         responseFactory?.Invoke(builder);
    //         HttpResponse = builder.Build();
    //         HttpResponse.StatusCode = statusCode;
    //         return this;
    //     }
    //
    //     /// <inheritdoc />
    //     public IWithResponse RespondTimes(int times, HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory = null)
    //     {
    //         return RespondTimes(times, (int) statusCode, responseFactory);
    //     }
    //
    //
    //     /// <inheritdoc />
    //     public IWithResponse WhichIsValidFor(int value, TimeUnit timeUnit = TimeUnit.Seconds)
    //     {
    //         TimeToLive = new LifeTime(value, timeUnit);
    //         return this;
    //     }
    //
    //     /// <inheritdoc />
    //     public MockServerSetup Setup()
    //     {
    //         _setup.Expectations.Add(_expectation);
    //         return _setup;
    //     }
    //
    //
    //     /// <inheritdoc />
    //     IFluentExpectationBuilder IWithResponse.And
    //     {
    //         get
    //         {
    //             Setup();
    //             return new FluentExpectationBuilder(_setup);
    //         }
    //     }
    // }
}
