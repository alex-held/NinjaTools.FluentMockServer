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
        
        [JsonProperty("delay")]
        public Delay Delay { get; set; }

        [JsonProperty("connectionOptions")]
        public ConnectionOptions ConnectionOptions { get; set; }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }
    }
}