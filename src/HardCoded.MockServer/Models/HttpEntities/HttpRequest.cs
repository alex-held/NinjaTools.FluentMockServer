using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HardCoded.MockServer.Fluent;
using HardCoded.MockServer.Models.HttpBodies;
using Newtonsoft.Json;

namespace HardCoded.MockServer.Models.HttpEntities
{
    public class HttpRequest
    {
        internal FluentExpectationBuilder Builder { get; }

        public HttpRequest()
        {
            Secure = false;
            KeepAlive = true;
        }
        
        internal HttpRequest(FluentExpectationBuilder fluentBuilder) : this()
        {
            Builder = fluentBuilder;
        }
        
        [JsonProperty("headers", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Headers { get; set; } 
        
        [JsonProperty("cookies", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Cookies { get; set; }
        
        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public RequestBody Body { get; set; }
        
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("secure")]
        public bool Secure { get; set; }

        [JsonProperty("keepAlive")]
        public bool KeepAlive { get; set; }

        public static HttpRequest Get(string path)
        {
           return new HttpRequest
           {
               Method = "GET",
               Path = path
           };
        }
    }
}