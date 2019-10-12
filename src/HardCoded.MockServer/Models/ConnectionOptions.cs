using HardCoded.MockServer.Contracts.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HardCoded.MockServer.Models
{
    public class ConnectionOptions : IBuildable
    {
        [JsonProperty("closeSocket", NullValueHandling = NullValueHandling.Ignore)]
        public bool CloseSocket { get; set; }

        [JsonProperty("contentLengthHeaderOverride", NullValueHandling = NullValueHandling.Ignore)]
        public long ContentLengthHeaderOverride { get; set; }

        [JsonProperty("suppressContentLengthHeader", NullValueHandling = NullValueHandling.Ignore)]
        public bool SuppressContentLengthHeader { get; set; }

        [JsonProperty("suppressConnectionHeader", NullValueHandling = NullValueHandling.Ignore)]
        public bool SuppressConnectionHeader { get; set; }

        [JsonProperty("keepAliveOverride", NullValueHandling = NullValueHandling.Ignore)]
        public bool KeepAliveOverride { get; set; }

        /// <inheritdoc />
        public JObject Serialize() =>
            JObject.FromObject(this);
    }
}