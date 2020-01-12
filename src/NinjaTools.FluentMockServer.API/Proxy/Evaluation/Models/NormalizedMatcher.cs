using System.Collections.Generic;
using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models
{
    public class NormalizedMatcher
    {
        public string? Method { get; private protected set; }
        public string? Path { get; private protected  set; }
        public string? Query { get; private protected set; }
        public Dictionary<string, string[]>? Headers { get; private protected set; }
        public Dictionary<string, string>? Cookies { get; private protected  set; }


        public bool IsHttps { get; private protected set; }
        
        public bool HasBody => Content != null;
        public string? Content { get; private protected  set; }
        public RequestBodyKind BodyKind { get; private protected  set; }
        public bool MatchExact { get; private protected set; }
    }

}
