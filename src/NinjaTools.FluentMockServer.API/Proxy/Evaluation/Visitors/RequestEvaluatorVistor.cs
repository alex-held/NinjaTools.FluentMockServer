using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Extensions;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Visitors
{
    public class RequestEvaluatorVistor  : IRequestMatcherEvaluatorVistor<IEvaluationResult>
    {

        private readonly EvaluationContext _evaluationContext;
        private HttpRequest HttpRequest => _evaluationContext.HttpContext.Request;

        public RequestEvaluatorVistor(EvaluationContext evaluationContext)
        {
            _evaluationContext = evaluationContext;
        }

        /// <inheritdoc />
        public IEvaluationResult Evaluate()
        {
            return _evaluationContext switch
            {
                { IsMatch: true } ctx => new EvaluationSuccessfulResult(ctx),
                { } ctx => new EvaluationUnsuccessfulResult(ctx)
            };
        }

        /// <inheritdoc />
        public void VisitCookies(IDictionary<string, string>? cookies)
        {
            if (_evaluationContext.EnsureNotNull(HttpRequest.Cookies, cookies) is {} httpCookies)
            {
                if (cookies.Except(httpCookies).Any())
                {
                    _evaluationContext.Fail(httpCookies, cookies);
                }
                else
                {
                    _evaluationContext.Match(httpCookies, cookies);
                }
            }
        }



        /// <inheritdoc />
        public void VisitHeaders(IDictionary<string, string[]>? headers)
        {
            if (_evaluationContext.EnsureNotNull(HttpRequest.Headers, headers) is {} httpRequestMember)
            {
                if (headers.Except(httpRequestMember.ToDictionary(k => k.Key, v => v.Value.ToArray())).Any())
                {
                    _evaluationContext.Fail(httpRequestMember, headers);
                }
                else
                {
                    _evaluationContext.Match(httpRequestMember, headers);
                }
            }
        }

        /// <inheritdoc />
        public void VisitPath(string? path)
        {
            if (_evaluationContext.EnsureNotNull(HttpRequest.Path.Value, path) is {} httpPath)
            {
                if(httpPath != path)
                {
                    _evaluationContext.Fail(httpPath, path);
                }
                else
                {
                    _evaluationContext.Match(httpPath, path);
                }
            }
        }

        /// <inheritdoc />
        public void VisitMethod(string? method)
        {
            if (_evaluationContext.EnsureNotNull(HttpRequest.Method, method) is {} httpMethod)
            {
                if (httpMethod != method)
                {
                    _evaluationContext.Fail(httpMethod, method);
                }
                else
                {
                    _evaluationContext.Match(httpMethod, method);
                }
            }
        }

        /// <inheritdoc />
        public void VisitQuery(string? query)
        {
            if (_evaluationContext.EnsureNotNull(HttpRequest.QueryString.Value, query) is {} httpQuery)
            {
                if (httpQuery != query)
                {
                    _evaluationContext.Fail(httpQuery, query);
                }
                else
                {
                    _evaluationContext.Match(httpQuery, query);
                }
            }
        }

        /// <inheritdoc />
        public void VisitBody(string? requestBody, bool exactMatch, RequestBodyKind kind)
        {
            HttpRequest.EnableBuffering();
            if (_evaluationContext.EnsureNotNull(HttpRequest.Body, requestBody) is { } httpBodyStream && httpBodyStream.CanSeek)
            {
                httpBodyStream.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(HttpRequest.Body);
                var httpBody = reader.ReadToEnd();

                if (requestBody is { } body && HttpRequest.Body.CanSeek)
                {
                    if (exactMatch && httpBody == body)
                    {
                        _evaluationContext.Match(httpBody, body);
                    }
                    else
                    {
                        _evaluationContext.Fail(httpBody, body);
                    }
                }
            }
        }
    }
}
