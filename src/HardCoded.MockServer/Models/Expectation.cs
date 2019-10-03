using System.Net.Http;
using HardCoded.MockServer.Models;
using HardCoded.MockServer.Models.HttpEntities;
using Newtonsoft.Json;

namespace HardCoded.MockServer
{
    public class Expectation
    {
        [JsonProperty("httpRequest")]
        public HttpRequest HttpRequest { get; set; }

        [JsonProperty("httpResponse")]
        public HttpResponse HttpResponse { get; set; }

        [JsonProperty("httpResponseTemplate", NullValueHandling = NullValueHandling.Ignore)]
        public HttpTemplate HttpResponseTemplate { get; set; }

        [JsonProperty("httpForward", NullValueHandling = NullValueHandling.Ignore)]
        public HttpForward HttpForward { get; set; }

        [JsonProperty("httpForwardTemplate", NullValueHandling = NullValueHandling.Ignore)]
        public HttpTemplate HttpForwardTemplate { get; set; }

        [JsonProperty("httpClassCallback", NullValueHandling = NullValueHandling.Ignore)]
        public HttpClassCallback HttpClassCallback { get; set; }

        [JsonProperty("httpObjectCallback", NullValueHandling = NullValueHandling.Ignore)]
        public HttpObjectCallback HttpObjectCallback { get; set; }

        [JsonProperty("httpError", NullValueHandling = NullValueHandling.Ignore)]
        public HttpError HttpError { get; set; }

        [JsonProperty("times", NullValueHandling = NullValueHandling.Ignore)]
        public Times Times { get; set; }

        [JsonProperty("timeToLive", NullValueHandling = NullValueHandling.Ignore)]
        public TimeToLive TimeToLive { get; set; }
    }
}
