using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace NinjaTools.FluentMockServer.API.Models.ViewModels
{
    public class MatchedRequest
    {
        [JsonIgnore] public HttpContext Context { get; }

        [JsonProperty(Order = 0)] public HttpRequestViewModel HttpRequest { get; }

        [JsonProperty(Order = 1)] public Setup Setup { get; }

        public MatchedRequest(HttpContext context, Setup setup)
        {
            Context = context;
            Setup = setup;
            HttpRequest = new HttpRequestViewModel(context.Request);
        }

        public static implicit operator (HttpContext, Setup)(MatchedRequest matchedRequest) => (matchedRequest.Context, matchedRequest.Setup);
        public static implicit operator MatchedRequest((HttpContext, Setup) matchedRequest) => new MatchedRequest(matchedRequest.Item1, matchedRequest.Item2);
    }
}
