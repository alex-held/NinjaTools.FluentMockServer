using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Models;

namespace NinjaTools.FluentMockServer.API.Proxy.Evaluation
{
    [DebuggerDisplay("{IsMatch} {Ratings} |  Messages={Messages}; Matcher={Matcher};", Name = "EvaluatingContext")]
    public class EvaluationContext
    {
        public EvaluationContext(HttpContext httpContext)
        {
            HttpContext = httpContext;
            Messages = new EvaluationMessages();
            Ratings = new EvaluationRatings();
            IsMatch = true;
        }

        public HttpContext HttpContext { get; }
        public NormalizedMatcher Matcher { get; }
        public EvaluationMessages Messages { get; }
        public EvaluationRatings Ratings { get; }
        public bool IsMatch { get; internal set; }
    }
}
