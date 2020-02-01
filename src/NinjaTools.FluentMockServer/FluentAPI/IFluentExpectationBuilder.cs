using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.FluentAPI
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [PublicAPI]
    public interface IBlankExpectation : IFluentInterface
    {
        [NotNull]
        IBlankExpectation UsingContext([NotNull] string context);

        [NotNull]
        IWithRequest OnHandling(HttpMethod method = null, [CanBeNull] Action<IFluentHttpRequestBuilder> requestFactory = null);

        [NotNull]
        IWithRequest OnHandlingAny([CanBeNull] HttpMethod method = null);
    }

    /// <inheritdoc cref="IBlankExpectation" />
    /// <summary>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [PublicAPI]
    public interface IFluentExpectationBuilder : IBlankExpectation , IWithRequest, IWithResponse
    {
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [PublicAPI]
    public interface IWithResponse : IFluentInterface
    {
        [NotNull]
        IFluentExpectationBuilder And { get; }
        [NotNull]
        IWithResponse WhichIsValidFor(int value, TimeUnit timeUnit = TimeUnit.Seconds);
        MockServerSetup Setup();
    }
        
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [PublicAPI]
    public interface IWithRequest : IFluentInterface
    {
        [NotNull]
        IWithResponse RespondWith(Action<IFluentHttpResponseBuilder> responseFactory);

        [NotNull]
        IWithResponse RespondWith(HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory = null);

        [NotNull]
        IWithResponse RespondOnce(Action<IFluentHttpResponseBuilder> responseFactory);

        [NotNull]
        IWithResponse RespondOnce(HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory = null);

        [NotNull]
        IWithResponse RespondTimes(int times, int statusCode, Action<IFluentHttpResponseBuilder> responseFactory = null);
    }
}
