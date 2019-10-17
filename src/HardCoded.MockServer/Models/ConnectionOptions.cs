using HardCoded.MockServer.Contracts.Abstractions;
using HardCoded.MockServer.Contracts.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HardCoded.MockServer.Models
{
    public class ConnectionOptions : BuildableBase
    {
        public bool CloseSocket { get; set; }

        public long ContentLengthHeaderOverride { get; set; }
        
        public bool SuppressContentLengthHeader { get; set; }

        public bool SuppressConnectionHeader { get; set; }

        public bool KeepAliveOverride { get; set; }
    }
}
