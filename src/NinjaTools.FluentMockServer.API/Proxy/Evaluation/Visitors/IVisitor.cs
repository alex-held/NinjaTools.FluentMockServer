namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Visitors
{
    public interface IRequestMatcherEvaluatorVistor<T> : IRequestMatcherEvaluatorVistor
    {
        T Evaluate();
    }

    public interface IVisitor
    {
    }
}
