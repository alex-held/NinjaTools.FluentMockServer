using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace NinjaTools.FluentMockServer.API.Models
{
    public class RequestMatcher
    {
        private string _path;

        public string Path
        {
            get => _path;
            set => _path = $"/{value?.TrimStart('/')}";
        }

        public string Method { get; set; }
        public Dictionary<string, string[]> Headers { get; set; }

        public bool IsMatch(HttpContext context)
        {
            var request = context.Request;
            return PathMatches(request.Path.Value + request.QueryString.Value)
                   && MethodMatches(request.Method)
                && HeadersMatching(request.Headers);
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
}
