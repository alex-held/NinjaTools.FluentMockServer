using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Visitors
{
    public interface IRequestMatcherEvaluatorVistor : IPartialVisitor
    {
        void Visit(RequestMatcher matcher);
    }
}