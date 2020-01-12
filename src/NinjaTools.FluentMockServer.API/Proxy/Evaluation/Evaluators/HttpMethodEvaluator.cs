using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators
{
    public class HttpMethodEvaluator : EvaluatorBase
    {
        /// <inheritdoc />
        protected override void EvaluateMember(EvaluationContext context)
        {
            var request = context.HttpContext.Request;
            var method = context.Matcher.Method;

            if (method is null)
            {
                context.Matches(EvaluationWeight.Low, $"Matched {nameof(RequestMatcher.Method)}. Value={method};");
            }
            else if (method == request.Method)
            {
                context.Matches(EvaluationWeight.Max, $"Matched {nameof(RequestMatcher.Method)}. Value={method};");
            }
            else
            {
                context.LogError(new ValidationException($"{nameof(HttpRequest.Method)} '{request.Method}' did not match setup. Expected={method ?? "*"}"));
            }
        }
    }
}
