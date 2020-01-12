using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation.Evaluators
{
    public class HttpHeaderEvaluator : EvaluatorBase
    {
        /// <inheritdoc />
        protected override void EvaluateMember(EvaluationContext context)
        {
            var request = context.HttpContext.Request;
            var requestHeaderDictionary = request.Headers;
            var headers = context.Matcher.Headers;

            if (headers is null)
            {
                context.Matches(EvaluationWeight.Low);
                return;
            }

            var requestHeaders = requestHeaderDictionary.ToDictionary(key => key.Key, val => val.Value.ToArray());
            var unsatisfiedHeaders = headers.Except(requestHeaders).ToList();

            if (!unsatisfiedHeaders.Any())
            {
                context.Matches(EvaluationWeight.Max);
                return;
            }

            context.LogError(new AmbiguousMatchException($"{nameof(HttpRequest)} didn't contain all configured Headers. {unsatisfiedHeaders.Count} Remaining={JObject.FromObject(headers).ToString(Formatting.Indented)};"));
        }
    }
}