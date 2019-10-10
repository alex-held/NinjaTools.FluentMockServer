using HardCoded.MockServer.Models.ValueTypes;
using Newtonsoft.Json;

namespace HardCoded.MockServer.Models.HttpEntities
{
    public class HttpResponse
    {
        public HttpResponse(int statusCode)
        {
            StatusCode = statusCode;
            ConnectionOptions = new ConnectionOptions();
            Delay = Delay.None;
        }
        
        [JsonProperty("delay", NullValueHandling = NullValueHandling.Ignore)]
        public Delay Delay { get; set; }

        [JsonProperty("connectionOptions", NullValueHandling = NullValueHandling.Ignore)]
        public ConnectionOptions ConnectionOptions { get; set; }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }
        
        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public Body Body { get; set; }
    }
}