using HardCoded.MockServer.Models.HttpEntities;
using HardCoded.MockServer.Models.ValueTypes;
using Newtonsoft.Json;

namespace HardCoded.MockServer
{
    public class Verify
    {
        [JsonProperty("httpRequest")]
        public HttpRequest HttpRequest { get; set; }
       
        [JsonProperty("times")]
        public Times Times { get; set; }
    }
}