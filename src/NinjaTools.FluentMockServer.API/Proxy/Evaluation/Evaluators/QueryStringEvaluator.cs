using NinjaTools.FluentMockServer.API.Extensions;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators
{
    public class QueryStringEvaluator : EvaluatorBase
    {
        /// <inheritdoc />
        protected override void EvaluateMember(EvaluationContext context)
        {
            var query = context.Matcher.Query;
            if (context.EnsureNotNull(context.HttpContext.Request.QueryString.Value, query) is {} httpQuery)
            {
                if (query is null)
                {
                    context.Match(httpQuery, query);
                    return;
                }
                context.Fail(httpQuery, query);
            }
        }
    }
}
