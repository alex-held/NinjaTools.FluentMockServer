using System;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Visitors
{
    public interface IRequest
    {
        void Accept(Func<IRequestMatcherEvaluatorVistor> visitorFactory);
        T Accept<T>(Func<IRequestMatcherEvaluatorVistor<T>> visitorFactory);
    }
}
