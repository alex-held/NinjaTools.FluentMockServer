using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HardCoded.MockServer.HttpBodies;
using Newtonsoft.Json;

namespace HardCoded.MockServer.Models.HttpEntities
{
    public class HttpRequest
    {
        
        public static HttpRequest Get(string path = null)
        {
            return new HttpRequest
            {
                Method = "GET",
                Path = path ?? "/",
            };
        }
        
        public static HttpRequest Post(string path = null)
        {
            return new HttpRequest
            {
                Method = "POST",
                Path = path ?? "/",
            };
        }
        
        public HttpRequest WithPath(string path)
        {
            Path = path;
            return this;
        }

        public HttpRequest UseSsl(bool useSsl = true)
        {
            Secure = useSsl;
            return this;
        }

        public HttpRequest KeepingAlive(bool keepAlive = true)
        {
            KeepAlive = keepAlive;
            return this;
        }

        public HttpRequest WithHeader(string name, params string[] values)
        {
            
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The provided cookie name must not be null or whitespace.", nameof(name));
            }   
            if (!values.Any())
            {
                throw new ArgumentException("The provided header values must contain at least one item.", nameof(values));
            } 
            
            Headers ??= new Dictionary<string, IEnumerable<string>>();
            Headers.TryAdd(name, values);
        
            return this;
        }
        
        public HttpRequest WithJson(string json)
        {
            
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentException("The provided json name must not be null or whitespace.", nameof(json));
            }   
            
            Body = RequestBody.FromJson(json);
        
            return this;
        }
        
        public HttpRequest WithCookie(string name, string value)
        {
            
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The provided cookie name must not be null or whitespace.", nameof(name));
            }   
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("The provided cookie value must not be null or whitespace.", nameof(value));
            }

            Cookies ??= new Dictionary<string, string>();
            Cookies.TryAdd(name, value);
            
            return this;
        }
        
        [JsonProperty("headers", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, IEnumerable<string>> Headers { get; set; } 
        
        [JsonProperty("cookies", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Cookies { get; set; }
        
        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public RequestBody Body { get; set; }
        
        
        public HttpRequest()
        {
            Secure = false;
            KeepAlive = true;
        }
        
        public HttpRequest(HttpMethod httpMethod, string path) : this()
        { 
            Method = httpMethod.ToString().ToUpper();
            Path = path;
        }
        
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("secure")]
        public bool Secure { get; set; }

        [JsonProperty("keepAlive")]
        public bool KeepAlive { get; set; }
    }
    
  

}