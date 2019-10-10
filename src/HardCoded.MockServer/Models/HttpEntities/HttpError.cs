using HardCoded.MockServer.Models.ValueTypes;
using Newtonsoft.Json;

namespace HardCoded.MockServer.Models.HttpEntities
{
    public partial class HttpError
    {
        [JsonProperty("delay")]
        public Delay Delay { get; set; }

        [JsonProperty("dropConnection")]
        public bool DropConnection { get; set; }

        [JsonProperty("responseBytes")]
        public string ResponseBytes { get; set; }
    }
}