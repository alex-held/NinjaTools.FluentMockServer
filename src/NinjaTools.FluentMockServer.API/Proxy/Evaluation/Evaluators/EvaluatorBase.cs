using System;
using System.Diagnostics;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators
{
    [DebuggerDisplay("{Name}; IsLast={IsLast};", Name = "Evaluator")]
    public abstract class EvaluatorBase : IEvaluator
    {
        protected virtual string FormatMatched(string subjectName, object match) => $"Matched {subjectName}. Value={match};";

        protected abstract void EvaluateMember(EvaluationContext context);


        public virtual string Name => GetType().Name;
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
            if (_next == null)
            {
                goto skip;
            }

            try
            {
                EvaluateMember(context);
                return _next.Evaluate(context);
            }
            catch (Exception e)
            {
                context.LogError(e);
            }

            skip:

            return context switch
            {
                { IsMatch: true } ctx => new EvaluationSuccessfulResult(ctx),
                { } ctx => new EvaluationUnsuccessfulResult(ctx)
            };
        }
    }
}