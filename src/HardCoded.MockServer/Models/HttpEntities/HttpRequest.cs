using System.Collections.Generic;
using System.Net.Http;
using HardCoded.MockServer.Contracts.Abstractions;
using HardCoded.MockServer.Contracts.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HardCoded.MockServer.Models.HttpEntities
{
    public class HttpRequest : IBuildable
    {
        /// <inheritdoc />
        public JObject Serialize() => JObject.FromObject(this);
        
        public static HttpRequest Get(string path)
        {
            return new HttpRequest
            {
                Method = HttpMethod.Get,
                Path = path
            };
        }
        
        public HttpRequest()
        {
            Secure = false;
            KeepAlive = true;
        }
        
        [JsonProperty("method")] 
        [JsonConverter(typeof(HttpMethodConverter))]
        public HttpMethod Method { get; set; }
        
        [JsonProperty("headers", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Headers { get; set; }

        [JsonProperty("cookies", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Cookies { get; set; }

        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public RequestBody? Body { get; set; }

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }

        [JsonProperty("secure", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Secure { get; set; }

        [JsonProperty("keepAlive", NullValueHandling = NullValueHandling.Ignore)] 
        public bool? KeepAlive { get; set; }
    }
}