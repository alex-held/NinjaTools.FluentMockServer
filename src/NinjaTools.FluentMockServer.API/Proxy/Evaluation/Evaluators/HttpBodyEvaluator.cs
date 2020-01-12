using System.IO;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Extensions;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators
{
    public class HttpBodyEvaluator : EvaluatorBase
    {
        /// <inheritdoc />
        protected override void EvaluateMember(EvaluationContext context)
        {
            if (context.EnsureNotNull(context.Matcher, context.HttpContext.Request.Body) is {} m)
            {

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
                    context.Match(content, bodyContent);
                    return;
                }
                else if (!shouldMatchExact && content.Length > 0 &&  bodyContent.Contains(content))
                {
                    context.Match(content, bodyContent);
                    return;
                }

                context.Fail(context.HttpContext.Request.Body, bodyContent);
            }
        }
    }
}
