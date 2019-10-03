using Newtonsoft.Json;

namespace HardCoded.MockServer.Models.HttpEntities
{
    public partial class HttpTemplate
    {
        [JsonProperty("template")]
        public string Template { get; set; }

        [JsonProperty("delay")]
        public Delay Delay { get; set; }
    }
}