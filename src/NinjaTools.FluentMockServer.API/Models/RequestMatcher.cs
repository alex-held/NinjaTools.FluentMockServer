using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Visitors;

namespace NinjaTools.FluentMockServer.API.Models
{

    [DebuggerDisplay("{DebuggerDisplay()}")]
    public class RequestMatcher : IRequest
    {
        public string DebuggerDisplay()
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(Method))
            {
                sb.Append($"Method={Method}; ");
            }

            if (!string.IsNullOrEmpty(Path))
            {
                sb.Append($"Path={Path}; ");
            }

            if (!string.IsNullOrEmpty(Query))
            {
                sb.Append($"Query={Query}; ");
            }

            sb.Append($"Headers={Headers?.Count ?? 0};\n");

            if (BodyMatcher != null)
            {
                sb.AppendLine(BodyMatcher.DebuggerDisplay());
            }

            return sb.ToString();
        }

        private string _path;

        public RequestBodyMatcher BodyMatcher { get; set; }

        public string Path
        {
            get => _path;
            set => _path = value is null
                ? null
                : $"/{value.TrimStart('/')}";
        }

        public string Method { get; set; }

        public Dictionary<string, string[]>? Headers { get; set; }
        public Dictionary<string, string>? Cookies { get; set; }


        public string? Query { get; set; }

        public bool IsMatch(HttpContext context)
        {
            var request = context.Request;
            return PathMatches(request.Path.Value)
                   && MethodMatches(request.Method)
                   && HeadersMatching(request.Headers)
                   && BodyMatches(request)
                   && QueryMatches(request.QueryString);
        }

        private bool QueryMatches(QueryString requestQueryString)
        {
            if (!string.IsNullOrWhiteSpace(Query))
            {
                return requestQueryString.Value == Query;
            }

            return true;
        }

        private bool BodyMatches(HttpRequest httpRequest)
        {

            if (BodyMatcher != null)
            {
                return BodyMatcher.IsMatch(httpRequest);
            }

            return true;
        }

        private bool PathMatches(string path)
        {
            if (string.IsNullOrWhiteSpace(Path))
            {
                return true;
            }

            return Path.StartsWith(path);
        }

        private bool MethodMatches(string requestMethod)
        {
            if (string.IsNullOrWhiteSpace(Method))
            {
                return true;
            }

            return requestMethod.ToUpper() == Method.ToUpper();
        }


        private bool HeadersMatching(IHeaderDictionary requestHeaders)
        {
            if (Headers is null)
            {
                return true;
            }

            foreach (var header in Headers)
            {

                if (!requestHeaders.ContainsKey(header.Key))
                {
                    return false;
                }
                
                if (requestHeaders.TryGetValue(header.Key, out var value))
                {
                    if (value.ToList().Except(header.Value).Any())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <inheritdoc />
        public void Accept(Func<IRequestMatcherEvaluatorVistor> visitorFactory)
        {
            var visitor = visitorFactory();
           this.BodyMatcher.Accept(() => visitor);

            visitor.VisitHeaders(Headers);
            visitor.VisitMethod(Method);
            visitor.VisitPath(Path);
            visitor.VisitQuery(Query);
            visitor.VisitCookies(Cookies);
            visitor.VisitBody(BodyMatcher);
            visitor.Visit(this);
        }

        /// <inheritdoc />
        public T Accept<T>(Func<IRequestMatcherEvaluatorVistor<T>> visitorFactory)
        {
            var  visitor = visitorFactory();
            Accept(()  => (IRequestMatcherEvaluatorVistor) visitor);
            return visitor.Evaluate();
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum RequestBodyKind
    {
        Text,
        Base64
    }
}
