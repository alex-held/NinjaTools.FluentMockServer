using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace NinjaTools.FluentMockServer.API.Models.ViewModels
{
    public class MatchedRequest
    {
        [JsonIgnore]
        public HttpContext Context { get; }

        [JsonProperty(Order = 0)]
        public HttpRequestViewModel HttpRequest { get; }

        [JsonProperty(Order = 1)]
        public Setup Setup { get; }

        public MatchedRequest(HttpContext context, Setup setup)
        {
            Context = context;
            Setup = setup;
            HttpRequest = new HttpRequestViewModel(context.Request);
        }
    }
}
