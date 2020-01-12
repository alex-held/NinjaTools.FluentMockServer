using System;
using System.Diagnostics;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators
{
    [DebuggerDisplay("{Name}; IsLast={IsLast};", Name = "Evaluator")]
    public abstract class EvaluatorBase : IEvaluator
    {
        public string Name => GetType().Name;
        protected abstract void EvaluateMember(EvaluationContext context);
        public bool IsLast => _next is null;

        private IEvaluator? _next;

        /// <inheritdoc />
        public IEvaluator SetNext(IEvaluator next)
        {
            _next = next;
            return next;
        }

        /// <inheritdoc />
        public virtual IEvaluationResult Evaluate(EvaluationContext context)
        {
            EvaluateMember(context);
            return _next != null ?  _next.Evaluate(context) : context switch
            {
                { IsMatch: true } ctx => new EvaluationSuccessfulResult(ctx),
                { } ctx => new EvaluationUnsuccessfulResult(ctx)
            };
        }
    }
}
