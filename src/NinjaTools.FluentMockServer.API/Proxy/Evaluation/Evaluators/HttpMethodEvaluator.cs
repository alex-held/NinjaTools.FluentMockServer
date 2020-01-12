using NinjaTools.FluentMockServer.API.Extensions;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators
{
    public class HttpMethodEvaluator : EvaluatorBase
    {
        /// <inheritdoc />
        protected override void EvaluateMember(EvaluationContext context)
        {
            var method = context.Matcher.Method;
            if (context.EnsureNotNull(context.HttpContext.Request.Method, method) is {} httpMethod)
            {
                if (method is null)
                {
                    context.Match(httpMethod, method);
                    return;
                }
                context.Fail(httpMethod, method);
            }
        }
    }
}
