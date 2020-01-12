using System.Collections.Generic;
using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Visitors
{
    public interface IPartialVisitor : IVisitor
    {
        void VisitCookies(IDictionary<string, string>? cookies);
        void VisitHeaders(IDictionary<string, string[]>? headers);
        void VisitPath(string? path);
        void VisitMethod(string? method);
        void VisitQuery(string? query);

        void VisitBody(RequestBodyMatcher? bodyMatcher);// => VisitBody(bodyMatcher.Content, bodyMatcher.MatchExact, bodyMatcher.Type);
    }
}
