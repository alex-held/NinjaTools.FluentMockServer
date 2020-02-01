using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace NinjaTools.FluentMockServer.API.Models.ViewModels
{
    public class MatchedRequest : IUpstreamRequest
    {
        [JsonConstructor]
        public MatchedRequest(HttpContext context, Setup setup)
        {
            Context = context;
            Setup = setup;
            HttpRequest = new HttpRequestViewModel(context.Request);
        }

        [JsonIgnore]
        public HttpContext Context { get; }

        /// <inheritdoc />
        [JsonIgnore]
        public bool WasMatched => true;

        public HttpRequestViewModel HttpRequest { get; }
        public Setup Setup { get; }
    }
}
