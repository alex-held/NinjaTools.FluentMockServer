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
            Ratings = context.Ratings;
        }

        /// <inheritdoc />
        public abstract bool IsMatch { get; }

        /// <inheritdoc />
        public EvaluationRatings Ratings { get; }

        public long Score => Ratings.Score;

        /// <inheritdoc />
        public IReadOnlyList<string> Messages { get; }

        /// <inheritdoc />
        public IReadOnlyList<Exception> Errors { get; }
    }
}
