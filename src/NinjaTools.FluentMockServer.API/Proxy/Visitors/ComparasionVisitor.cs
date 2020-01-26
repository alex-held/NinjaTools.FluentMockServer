using System.IO;
using System.Linq;
using System.Threading;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Extensions;
using NinjaTools.FluentMockServer.API.Models;
using Path = NinjaTools.FluentMockServer.API.Models.Path;

namespace NinjaTools.FluentMockServer.API.Proxy.Visitors
{
    /// <inheritdoc cref="IVisitor" />
    public sealed class ComparasionVisitor : IVisitor,
        IVisitor<RequestMatcher>,
        IVisitor<Headers>,
        IVisitor<Cookies>,
        IVisitor<Path>,
        IVisitor<Query>,
        IVisitor<Method>,
        IVisitor<RequestBodyMatcher>
    {
        private HttpContext HttpContext { get; }
        private HttpRequest HttpRequest => HttpContext.Request;
        public bool IsSuccess { get; set; } = true;
        public double Score { get; set; }
        private static double Pass() => 1;

        private double Fail()
        {
            IsSuccess = false;
            return 0;
        }

        public ComparasionVisitor(HttpContext httpContext)
        {
            HttpContext = httpContext;
        }

        /// <inheritdoc cref="IVisitor{TVisitable}" />
        /// <remarks> ✅ Entrypoint ✅ </remarks>
        public double Visit(RequestMatcher visitable, CancellationToken token = default)
        {
            Score += Visit(visitable.Cookies);
            Score += Visit(visitable.Headers);
            Score += Visit(visitable.Query);
            Score += Visit(visitable.Path);
            Score += Visit(visitable.Method);
            Score += Visit(visitable.BodyMatcher);
            return Score;
        }

        /// <inheritdoc />
        public double Visit(RequestBodyMatcher? visitable, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            if (!visitable?.IsEnabled() ?? true)
                return Pass();

            Guard.Against.Null(visitable, nameof(visitable) + nameof(RequestBodyMatcher));
            if (HttpRequest.Body is null)
                return Fail();

            HttpRequest.EnableBuffering();
            HttpRequest.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(HttpRequest.Body);
            var httpBody = reader.ReadToEnd();

            if (visitable.MatchExact.HasValue && visitable.MatchExact.Value  && httpBody == visitable.Content)
                return Pass();

            return Fail();
        }

        /// <inheritdoc />
        public double Visit(Headers? visitable, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            if (!visitable?.Any() ?? true)
                return Pass();

            Guard.Against.NullOrEmpty(visitable.Header, nameof(visitable) + nameof(Headers));

            if (HttpRequest.Headers is null && visitable.Header.EnsureNotNullNotEmpty() is { })
                return Fail();

            var success = visitable.Header.Where(v =>
            {
                if (!HttpContext.Request.Headers.TryGetValue(v.Key, out var value))
                    return false;

                return !value.Except(v.Value).Any();

            }).Any();

            return success ? Pass() : Fail();
        }


        /// <inheritdoc />
        public double Visit(Cookies? visitable, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            if (!visitable?.Any() ?? true)
                return Pass();

            Guard.Against.NullOrEmpty(visitable.Cookie, nameof(visitable) + nameof(Cookies));

            if (HttpRequest.Cookies is null && visitable.Cookie.EnsureNotNullNotEmpty().Any())
                Fail();

            if (visitable.Cookie.Except(HttpContext.Request.Cookies).EnsureNotNullNotEmpty().Any())
                return Fail();

            return Pass();
        }

        /// <inheritdoc />
        public double Visit(Query visitable, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            if (!visitable?.IsEnabled() ?? true)
                return Pass();

            Guard.Against.NullOrEmpty(visitable.QueryString.Value, nameof(visitable) + nameof(Query));
            if (!HttpRequest.QueryString.HasValue && visitable.QueryString.HasValue)
                return Fail();

            if (visitable.QueryString.Value == HttpRequest.QueryString.Value)
                return Pass();

            return Fail();
        }

        /// <inheritdoc />
        public double Visit(Path? visitable, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            if (!visitable?.IsEnabled() ?? true)
                return Pass();

            Guard.Against.NullOrWhiteSpace(visitable, nameof(visitable) + nameof(Path));

            if (!HttpRequest.Path.HasValue && visitable is { } v && v.PathString.HasValue)
                return  Fail();

            if (visitable.ToPath() == HttpRequest.Path.Value)
                return Pass();

            return  Fail();
        }

        /// <inheritdoc />
        public double Visit(Method? visitable, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            if (!visitable?.IsEnabled() ?? true)
                return Pass();

            Guard.Against.NullOrEmpty(visitable, nameof(visitable) + nameof(Method));
            if (string.IsNullOrWhiteSpace(HttpRequest.Method) && visitable is { } v && !string.IsNullOrWhiteSpace(v.MethodString))
                return Fail();

            if (visitable.MethodString == HttpRequest.Method)
                return Pass();

            return Fail();
        }
    }
}
