using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace HardCoded.MockServer.Models.HttpEntities
{
    public class HttpRequest
    {
        public static HttpRequest Get(string path)
        {
            return new HttpRequest
            {
                Method = "GET", Path = path
            };
        }
        
        public HttpRequest()
        {
            Secure = false;
            KeepAlive = true;
        }
        
        [JsonProperty("headers", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Headers { get; set; }

        [JsonProperty("cookies", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Cookies { get; set; }

        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public Body Body { get; set; }

        [JsonProperty("path")] public string Path { get; set; }

        [JsonProperty("method")] public string Method { get; set; }

        [JsonProperty("secure")]
        public bool Secure { get; set; }

        [JsonProperty("keepAlive")] 
        public bool KeepAlive { get; set; }
    }
}