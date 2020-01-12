namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Visitors
{
    public interface IRequestMatcherEvaluatorVistor<T>
    {
        T Evaluate();
    }

    public interface IVisitor
    {
    }
}
