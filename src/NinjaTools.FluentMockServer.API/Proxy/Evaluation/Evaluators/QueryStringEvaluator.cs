using System.Reflection;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators
{
    public class QueryStringEvaluator : EvaluatorBase
    {
        /// <inheritdoc />
        protected override void EvaluateMember(EvaluationContext context)
        {
            var request = context.HttpContext.Request;
            var requestQueryString = request.QueryString;
            var query = context.Matcher.Query;

            if (query is null)
            {
                context.Matches(EvaluationWeight.Low);
                return;
            }
            else if(query == request.QueryString.ToString())
            {
                context.Matches(EvaluationWeight.Max);
                return;
            }

            context.LogError(new AmbiguousMatchException($"Actual QueryString '{requestQueryString.ToString()}' didn't match. Expected={query};"));
        }
    }
}