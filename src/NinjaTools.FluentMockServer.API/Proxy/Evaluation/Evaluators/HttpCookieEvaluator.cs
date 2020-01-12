using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators
{
    public class HttpCookieEvaluator : EvaluatorBase
    {
        /// <inheritdoc />
        protected override void EvaluateMember(EvaluationContext context)
        {
            var request = context.HttpContext.Request;
            var requestCookies = request.Cookies;
            var cookies = context.Matcher.Cookies;

            if (cookies is null)
            {
                context.Matches(EvaluationWeight.Low);
                return;
            }

            var unsatisfiedCookies = cookies.Except(requestCookies).ToList();

            if (unsatisfiedCookies.Any() != true)
            {
                context.Matches(EvaluationWeight.High);
                return;
            }

            context.LogError(new AmbiguousMatchException($"{nameof(HttpRequest)} didn't contain all configured Headers. {unsatisfiedCookies.Count} Remaining={JObject.FromObject(cookies).ToString(Formatting.Indented)};"));
        }
    }
}