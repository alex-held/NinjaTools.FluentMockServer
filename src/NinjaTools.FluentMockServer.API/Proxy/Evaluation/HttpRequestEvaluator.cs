using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation
{
    public class HttpRequestEvaluator : IEvaluator
    {
        private  IEvaluator _evaluator;

        /// <inheritdoc />
        public IEvaluator SetNext(IEvaluator next)
        {
            _evaluator = next;
            return next;
        }

        /// <inheritdoc />
        public IEvaluationResult Evaluate(EvaluationContext context)
        {
            return _evaluator.Evaluate(context);
        }
    }
}
