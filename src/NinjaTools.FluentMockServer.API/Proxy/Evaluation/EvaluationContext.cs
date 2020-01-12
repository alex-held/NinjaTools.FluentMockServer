using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation
{
    [DebuggerDisplay("{IsMatch} {Ratings} |  Messages={Messages}; Matcher={Matcher};", Name = "EvaluatingContext")]
    public class EvaluationContext
    {
        public EvaluationContext(HttpContext httpContext, NormalizedMatcher matcher)
        {
            HttpContext = httpContext;
            Matcher = matcher;
            Messages = new EvaluationMessages();
            Ratings = new EvaluationScore();
        }

        public HttpContext HttpContext { get; }
        public NormalizedMatcher Matcher { get; }
        public EvaluationMessages Messages { get; }
        public EvaluationScore Ratings { get; }
        public bool IsMatch { get; private set; } = true;

        public void AddExtraPoints(uint bonus) => Ratings.BonusPoints += bonus;


        /// <param name="weight"></param>
        public void Matches(EvaluationWeight weight, string? message = null)
        {
            Ratings.Add(weight);
            if (message != null)
            {
                Messages.Messages.Add(message);
            }
        }

        public void LogError(Exception exception)
        {
            IsMatch = false;
            Messages.Exceptions.Add(exception);
        }
    }
}