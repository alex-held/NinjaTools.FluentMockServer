using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;

using HardCoded.MockServer.Contracts.Abstractions;
using HardCoded.MockServer.Contracts.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace HardCoded.MockServer.Models.HttpEntities
{
    public class HttpRequest : BuildableBase
    {
        public HttpMethod Method { get; set; }
        
        public Dictionary<string, object> Headers { get; set; }
        
        public Dictionary<string, string> Cookies { get; set; }
        public RequestBody? Body { get; set; }
        public string Path { get; set; }
        public bool? Secure { get; set; }
        public bool? KeepAlive { get; set; }
    }
}
