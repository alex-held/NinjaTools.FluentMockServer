using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators
{
    public class HttpBodyEvaluator : EvaluatorBase
    {
        /// <inheritdoc />
        protected override void EvaluateMember(EvaluationContext context)
        {
            if (context.Matcher.HasBody != true)
            {
                context.Matches(EvaluationWeight.Low);
                return;
            }

            var request = context.HttpContext.Request;

            var bodyKind = context.Matcher.BodyKind;
            var shouldMatchExact = context.Matcher.MatchExact;
            var bodyContent = context.Matcher.Content;


            request.EnableBuffering();
            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body);
            var content = reader.ReadToEnd();

            if (shouldMatchExact && bodyContent == content)
            {
                context.Matches(EvaluationWeight.Max);
                context.AddExtraPoints(3);

            }
            else if (!shouldMatchExact && content.Length > 0 &&  bodyContent.Contains(content))
            {
                context.Matches(EvaluationWeight.Max);
            }
            else
            {
                context.LogError(new AmbiguousMatchException($"{nameof(HttpRequest.Body)} '{content}' did not match. Expected={bodyContent};"));
            }
        }
    }
}
