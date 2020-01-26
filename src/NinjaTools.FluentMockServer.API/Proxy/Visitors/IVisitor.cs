using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NinjaTools.FluentMockServer.API.Proxy.Visitors.Collections;
using QueryCollection = NinjaTools.FluentMockServer.API.Proxy.Visitors.Collections.QueryCollection;

namespace NinjaTools.FluentMockServer.API.Models
{
    public class RequestBodyMatcher
    {
        public string? Content { get; set; }
        public RequestBodyKind  Kind { get; set; }
        public bool MatchExact { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum RequestBodyKind
    {
        Text,
        Base64
    }
    public class RequestMatcher
    {
        public RequestBodyMatcher? BodyMatcher { get; set; }
        public HttpMethodWrapper? Method { get; set; }
        public HeaderCollection? Headers { get; set; }
        public CookieCollection? Cookies { get; set; }
        public QueryCollection? Query { get; set; }
        public PathCollection? Path { get; set; }
    }
}

namespace NinjaTools.FluentMockServer.API.Proxy.Visitors
{
    public interface IVisitor
    { }

    public interface IVisitor<TVisitable>
    {
        void Visit(TVisitable visitable) => Visit(visitable,default);
        void Visit(TVisitable visitable, CancellationToken token);
    }

    public interface IVisitable
    {
        void Accept(IVisitor visitor);
    }
}
