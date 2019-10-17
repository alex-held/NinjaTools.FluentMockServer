using HardCoded.MockServer.Contracts.Abstractions;
using HardCoded.MockServer.Models.HttpEntities;
using HardCoded.MockServer.Models.ValueTypes;

using Newtonsoft.Json;


namespace HardCoded.MockServer.Models
{
    public class Expectation : BuildableBase
    {
        public HttpRequest HttpRequest { get; set; }
        public HttpResponse HttpResponse { get; set; }
        public HttpTemplate HttpResponseTemplate { get; set; }
        public HttpForward HttpForward { get; set; }
        public HttpTemplate HttpForwardTemplate { get; set; }
        public HttpError HttpError { get; set; }
        public Times Times { get; set; }
        public TimeToLive TimeToLive { get; set; }
    }
}
