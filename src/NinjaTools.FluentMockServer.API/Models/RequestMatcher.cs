using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace NinjaTools.FluentMockServer.API.Models
{
    public class RequestMatcher
    {
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
        public Dictionary<string, string[]> Headers { get; set; }


        public QueryString? QueryString { get; set; }

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
            if (QueryString.HasValue)
            {
                return requestQueryString.Value == QueryString.Value.Value;
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
    }

    public class RequestBodyMatcher
    {
        [CanBeNull]
        public string Content { get; set; }

        public RequestBodyType  Type { get; set; }
        public bool MatchExact { get; set; }


        public bool IsMatch(HttpRequest request)
        {
            request.EnableBuffering();
            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body);
            var content = reader.ReadToEnd();

            if (MatchExact)
            {
                return Content == content;
            }


            return content.Contains(Content);
        }
    }

    public enum RequestBodyType
    {
        Text,
        Base64
    }
    
}
