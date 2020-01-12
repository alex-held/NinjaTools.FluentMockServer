using System.Linq;
using NinjaTools.FluentMockServer.API.Extensions;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators
{
    public class HttpHeaderEvaluator : EvaluatorBase
    {
        /// <inheritdoc />
        protected override void EvaluateMember(EvaluationContext context)
        {
            var header = context.Matcher.Headers;
            if (context.EnsureNotNull(context.HttpContext.Request.Headers, header) is {} httpHeader)
            {
                var unsatisfiedHeaders = header.Except(httpHeader
                    .ToDictionary(k => k.Key, v => v.Value.ToArray()));


                if (unsatisfiedHeaders.Any() != true)
                {
                    context.Match(httpHeader, header);
                    return;
                }

                context.Fail(httpHeader, header);
            }
        }
    }
}
