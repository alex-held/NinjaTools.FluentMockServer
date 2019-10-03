using Newtonsoft.Json;

namespace HardCoded.MockServer.Models.HttpEntities
{
    public partial class HttpForward
    {
        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("port")]
        public long Port { get; set; }
    }
}