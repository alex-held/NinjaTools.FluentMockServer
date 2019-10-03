using Newtonsoft.Json;

namespace HardCoded.MockServer
{
    public partial class ConnectionOptions
    {
        [JsonProperty("closeSocket")]
        public bool CloseSocket { get; set; }

        [JsonProperty("contentLengthHeaderOverride")]
        public long ContentLengthHeaderOverride { get; set; }

        [JsonProperty("suppressContentLengthHeader")]
        public bool SuppressContentLengthHeader { get; set; }

        [JsonProperty("suppressConnectionHeader")]
        public bool SuppressConnectionHeader { get; set; }

        [JsonProperty("keepAliveOverride")]
        public bool KeepAliveOverride { get; set; }
    }
}