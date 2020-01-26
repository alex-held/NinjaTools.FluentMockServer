using System;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.API.Proxy.Visitors.Collections;
using QueryCollection = NinjaTools.FluentMockServer.API.Proxy.Visitors.Collections.QueryCollection;
// ReSharper disable PossibleLossOfFraction

namespace NinjaTools.FluentMockServer.API.Proxy.Visitors
{
    public class ComparasionVisitor : IVisitor,
        IVisitor<RequestMatcher>,
        IVisitor<HeaderDictionary>,
        IVisitor<CookieCollection>,
        IVisitor<PathCollection>,
        IVisitor<QueryCollection>,
        IVisitor<HttpMethodWrapper>,
        IVisitor<RequestBodyMatcher>
    {
        protected HttpContext HttpContext { get; }
        protected HttpRequest HttpRequest => HttpContext.Request;
        public bool IsSuccess { get; set; } = true;
        public double Score { get; set; }  = 0;


        protected virtual void Fail()
        {
            IsSuccess = false;
            //throw new ExecutionEngineException("FAIL");
        }

        protected virtual double? Pass()
        {
            Score += 1;
            return Score;
        }

        public ComparasionVisitor(HttpContext httpContext)
        {
            HttpContext = httpContext;
        }

        /// <inheritdoc />
        /// <remarks> ✅ Entrypoint ✅ </remarks>
        public void Visit(RequestMatcher visitable, CancellationToken token = default)
        {
            Visit(visitable.Cookies);
            Visit(visitable.Headers);
            Visit(visitable.Query);
            Visit(visitable.Path);
            Visit(visitable.Method);
            Visit(visitable.BodyMatcher);
        }

        /// <inheritdoc />
        public void Visit(RequestBodyMatcher visitable, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            if (visitable is null || string.IsNullOrWhiteSpace(visitable.Content))
            {
                Pass();
                return;
            }

            if (HttpRequest.Body is null)
            {
                Fail();
                return;
            }

            HttpRequest.EnableBuffering();
            HttpRequest.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(HttpRequest.Body);
            var httpBody = reader.ReadToEnd();

            if (visitable.MatchExact && httpBody == visitable.Content)
            {
                Pass();
                return;
            }

            Fail();

        }

        /// <inheritdoc />
        public void Visit(HeaderDictionary visitable, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            if (visitable is null)
            {
                Pass();
                return;
            }

            if (HttpRequest.Headers is null && visitable is { } v && v.Any())
            {
                Fail();
                return;
            }

            var rest = visitable?.Except(HttpRequest.Headers);

            if (rest is {} r && r.Any() )
            {
                Fail();
                return;
            }

            Pass();
            return;

        }

        /// <inheritdoc />
        public void Visit(CookieCollection visitable, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            if (visitable is null)
            {
                Pass();
                return;
            }
            if (HttpRequest.Cookies is null && visitable is { } v && v.Any())
            {
                Fail();
                return;
            }

            var rest = visitable?.Except(HttpRequest.Cookies);

            if (rest is {} r && r.Any() )
            {
                Fail();
                return;
            }
            Pass();
            return;
        }

        /// <inheritdoc />
        public void Visit(QueryCollection visitable, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            if (visitable is null)
            {
                Pass();
                return;
            }

            if (!HttpRequest.QueryString.HasValue && visitable is { } v && v.QuryString.HasValue)
            {
                Fail();
                return;
            }
            if (visitable?.Query == HttpRequest.QueryString.Value)
            {
                Pass();
                return;
            }

            Fail();
        }

        /// <inheritdoc />
        public void Visit(PathCollection visitable, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            if (visitable is null)
            {
                Pass();
                return;
            }
             if (!HttpRequest.Path.HasValue && visitable is { } v && v.PathString.HasValue)
            {
                Fail();
                return;
            }
             if (visitable?.Path == HttpRequest.Path.Value)
            {
                Pass();
                return;
            }

             Fail();
             return;

        }

        /// <inheritdoc />
        public void Visit(HttpMethodWrapper visitable, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            if (visitable is null)
            {
                Pass();
                return;
            }
             if (string.IsNullOrWhiteSpace(HttpRequest.Method) && visitable is { } v && !string.IsNullOrWhiteSpace(v.MethodString))
            {
                Fail();
                return;
            }
             if (visitable.MethodString == HttpRequest.Method)
            {
                Pass();
                return;
            }

                Fail();
        }
    }
}
