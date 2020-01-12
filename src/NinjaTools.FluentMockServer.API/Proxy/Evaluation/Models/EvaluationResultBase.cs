using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models
{
    public abstract class EvaluationResultBase : IEvaluationResult
    {
        public EvaluationResultBase([NotNull] EvaluationContext context)
        {
            Messages = context.Messages.Messages;
            Errors = context.Messages.Exceptions;
            Score = context.Ratings.Score;
        }

        /// <inheritdoc />
        public abstract bool IsMatch { get; }
        /// <inheritdoc />
        public uint Score { get; }

        /// <inheritdoc />
        public IReadOnlyList<string> Messages { get; }

        /// <inheritdoc />
        public IReadOnlyList<Exception> Errors { get; }

        /// <inheritdoc />
        public int ErrorCount => Errors.Count;
    }
}