using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation
{
    public interface IEvaluator
    {
        IEvaluator SetNext(IEvaluator next);

        IEvaluationResult Evaluate(EvaluationContext context);
    }
}