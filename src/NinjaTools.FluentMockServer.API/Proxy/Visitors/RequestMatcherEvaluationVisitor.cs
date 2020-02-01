using System;
using System.Diagnostics;
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
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "}")]
    public sealed class RequestMatcherEvaluationVisitor :
        IRequestMatcherEvaluationVisitor,
        IVisitor<Headers>,
        IVisitor<Cookies>,
        IVisitor<Path>,
        IVisitor<Query>,
        IVisitor<Method>,
        IVisitor<RequestBodyMatcher>
    {
        public string DebuggerDisplay() => $"Cancelled={_cts.IsCancellationRequested}; [Upstream Info]  Path={HttpRequest.Path.ToString()}; Method={HttpRequest.Method};";

        /// <summary>
        /// Initializes a new instance of <see cref="RequestMatcherEvaluationVisitor"/>.
        /// </summary>
        /// <param name="httpContext">The <see cref="HttpContext"/> of the current ASPNETCORE scope.></param>
        public RequestMatcherEvaluationVisitor(HttpContext httpContext)
        {
            _cts = new CancellationTokenSource();
            HttpContext = httpContext;
        }

        private CancellationTokenSource _cts;
        private HttpContext HttpContext { get; }
        private HttpRequest HttpRequest => HttpContext.Request;

        /// <inheritdoc />
        public int Visit(RequestMatcher visitable)
        {
            try
            {
                _cts ??= new CancellationTokenSource();
                var token = _cts.Token;

                var score = 0;
                score += Visit(visitable.Cookies, token);
                score += Visit(visitable.Headers, token);
                score += Visit(visitable.Query, token);
                score += Visit(visitable.Path, token);
                score += Visit(visitable.Method, token);
                score += Visit(visitable.BodyMatcher, token);

                token.ThrowIfCancellationRequested();
                return score;
            }
            catch (OperationCanceledException canceled)
            {
                _cts = null;
                return 0;
            }
        }

        #region VisitMembers

         /// <inheritdoc />
        public int Visit(Cookies visitable, CancellationToken token = default)
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
        public int Visit(Headers visitable, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            if (!visitable?.Any() ?? true)
                return Pass();

            Guard.Against.NullOrEmpty(visitable.Header, nameof(visitable) + nameof(Headers));

            if (HttpRequest.Headers is null && visitable.Header.EnsureNotNullNotEmpty() is { })
                return Fail();

            var success = visitable.Header
                .Select(h =>
                {
                    if (!HttpContext.Request.Headers.TryGetValue(h.Key, out var value))
                        return 0;

                    return !value.Except(h.Value).Any() ? 1 : 0;
                })
                .Sum();

            return success > 0
                ? success
                : Fail();
        }

        /// <inheritdoc />
        public int Visit(Method visitable, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            if (!visitable?.HasContent() ?? true)
                return Pass();

            Guard.Against.NullOrEmpty(visitable, nameof(visitable) + nameof(Method));
            if (string.IsNullOrWhiteSpace(HttpRequest.Method) && visitable is { } v && !string.IsNullOrWhiteSpace(v.MethodString))
                return Fail();

            return visitable.MethodString == HttpRequest.Method ? Pass() : Fail();
        }

        /// <inheritdoc />
        public int Visit(Path visitable, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            if (!visitable?.HasContent() ?? true)
                return Pass();

            Guard.Against.NullOrWhiteSpace(visitable, nameof(visitable) + nameof(Path));

            if (!HttpRequest.Path.HasValue && visitable is { } v && v.PathString.HasValue)
                return Fail();

            return visitable.ToPath() == HttpRequest.Path.Value ? Pass() : Fail();
        }

        /// <inheritdoc />
        public int Visit(Query visitable, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            if (visitable is null || !visitable.HasContent())
                return Pass();

            Guard.Against.NullOrEmpty(visitable.QueryString.Value, nameof(visitable) + nameof(Query));
            if (!HttpRequest.QueryString.HasValue && visitable.QueryString.HasValue)
                return Fail();

            return visitable.QueryString.Value == HttpRequest.QueryString.Value ? Pass() : Fail();
        }

        /// <inheritdoc />
        public int Visit(RequestBodyMatcher visitable, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            if (!visitable?.HasContent() ?? true)
                return Pass();

            Guard.Against.Null(visitable, nameof(visitable) + nameof(RequestBodyMatcher));
            if (HttpRequest.Body is null)
                return Fail();

            HttpRequest.EnableBuffering();
            HttpRequest.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(HttpRequest.Body);
            var httpBody = reader.ReadToEnd();

            if (visitable.IsExactMatch)
            {
                if (visitable.Content is {} body && body == httpBody) return Pass();

                return Fail();
            }

            else
            {
                if (visitable.Content is {} body && httpBody.Contains(body)) return Pass();

                return Fail();
            }
        }


        #endregion


        private static int Pass() => 1;

        private int Fail()
        {
            _cts.Cancel();
            return 0;
        }
    }
}
