using Newtonsoft.Json;

namespace HardCoded.MockServer.Models.HttpEntities
{
    public partial class HttpClassCallback
    {
        [JsonProperty("callbackClass")]
        public string CallbackClass { get; set; }
    }
}