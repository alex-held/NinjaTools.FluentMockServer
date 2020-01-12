using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Extensions;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators
{
    public class HttpPathEvaluator : EvaluatorBase
    {

        /// <inheritdoc />
        protected override void EvaluateMember(EvaluationContext context)
        {
            var request = context.HttpContext.Request;
            var requestPath = request.Path.Value;
            var path = context.Matcher.Path;

            if (path is null)
            {
                context.Matches(EvaluationWeight.Low);
                return;
            }

            if (path.IsValidRegex() &&Regex.IsMatch(requestPath, path, RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace))
            {
                context.Matches(EvaluationWeight.High);
                return;
            }

            context.LogError(new AmbiguousMatchException($"{nameof(HttpRequest.Path)}-Regex '{path}' did not match. Actual={requestPath};"));;
        }
    }
}