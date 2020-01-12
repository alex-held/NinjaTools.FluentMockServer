using System.Linq;
using NinjaTools.FluentMockServer.API.Extensions;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators
{
    public class HttpCookieEvaluator : EvaluatorBase
    {
        /// <inheritdoc />
        protected override void EvaluateMember(EvaluationContext context)
        {
            var cookies = context.Matcher.Cookies;
            if (context.EnsureNotNull(context.HttpContext.Request.Cookies, cookies) is {} httpCookies)
            {
                var unsatisfiedCookies = cookies.Except(httpCookies).ToList();

                if (unsatisfiedCookies.Any() != true)
                {
                    context.Match(httpCookies, cookies);
                    return;
                }
                context.Fail(httpCookies, cookies);
            }
        }
    }
}
