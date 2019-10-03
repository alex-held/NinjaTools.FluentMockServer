using Newtonsoft.Json;

namespace HardCoded.MockServer.Models.HttpEntities
{
    public partial class HttpObjectCallback
    {
        [JsonProperty("clientId")]
        public string ClientId { get; set; }
    }
}