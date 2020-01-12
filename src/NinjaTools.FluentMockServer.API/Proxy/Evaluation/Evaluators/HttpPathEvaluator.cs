using NinjaTools.FluentMockServer.API.Extensions;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators
{
    public class HttpPathEvaluator : EvaluatorBase
    {

        /// <inheritdoc />
        protected override void EvaluateMember(EvaluationContext context)
        {
            var path = context.Matcher.Path;
            if (context.EnsureNotNull(context.HttpContext.Request.Path.Value, path) is {} httpPath)
            {
                if (path is null)
                {
                    context.Match(httpPath, path);
                    return;
                }
                context.Fail(httpPath, path);
            }
        }
    }
}
