using System;
using System.Collections.Generic;
using System.Net.Http;
using FluentApi.Generics.Framework;
using HardCoded.MockServer.Fluent.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HardCoded.MockServer.Fluent.Builder
{

    
    public class HttpRequest
    {
        [JsonProperty("headers", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Headers { get; set; } 
        
        [JsonProperty("cookies", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Cookies { get; set; }
        
        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public HttpRequestBuilder.Body Body { get; set; }
        
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("secure",NullValueHandling = NullValueHandling.Ignore)]
        public bool Secure { get; set; }

        [JsonProperty("keepAlive")]
        public bool KeepAlive { get; set; }

        /// <summary>
        /// Builder to configure and build a <see cref="HttpRequest"/>.
        /// </summary>
        /// <returns></returns>
        public static HttpRequestBuilder.IBlank Configure()
        {
            return new HttpRequestBuilder();
        }

       
        
        /// The builder for <see cref="HttpRequest" />
        public class HttpRequestBuilder : HttpRequestBuilder.IBlank, HttpRequestBuilder.IBuildable
        {
            private Dictionary<string, object> Headers { get; set; } 
        
            private Dictionary<string, string> Cookies { get; set; }
        
            private Body Content { get; set; }
        
            private string Path { get; set; }

            private string Method { get; set; }

            private bool Secure { get; set; }
            private bool KeepAlive { get; set; }
            
            private string HttpMethod { get; set; }
            
            
            public class Body : Dictionary<string, object>
            {
                public static Body For(BodyType type) => new Body(type);

                public Body()
                {
                }
                
                private Body(BodyType type)
                {
                    Add("type", type.ToString().ToUpper());
                }
                
                [JsonConverter(typeof(StringEnumConverter))]
                public enum BodyType
                {
                    JSON_SCHEMA,
                    JSON_PATH,
                    JSON,
                    STRING,
                    XML_SCHEMA,
                    XML,
                    XPATH,
                    REGEX,
                    BINARY
                }
        
        
            }
            
            internal HttpRequestBuilder()
            {
            }

            public interface IBuildable : IBlank, IWithHeader, IWithCookie, IWithContent
            {
                HttpRequest Build();
                IBuildable EnableEncryption(bool useSsl = true);
                IBuildable KeepConnectionAlive(bool keepAlive = true);
            }

            public interface IBlank 
            {
                IBuildable WithPath(string path);
                IBuildable WithMethod(HttpMethod method);
            }
            
            public interface IWithContent
            {
                IBuildable WithJsonContent(string content);
                IBuildable WithXmlContent(string content);
                IBuildable WithBinaryContent(byte[] content);
                IBuildable WithJsonArray<T>(params T[] items);
                IBuildable WithJson<T>(T item);
                IBuildable MatchExactJsonContent(string content);

            }
      

            public interface IWithHeader
            {
                IBuildable WithHeader(string name, object value);
            }

            public interface IWithCookie
            {
                IBuildable WithCookie(string name, string value);
            }

            /// <inheritdoc />
            public IBuildable WithMethod(HttpMethod method)
            {
                HttpMethod = method.ToString().ToUpper();
                return this;
            }
            
            /// <inheritdoc />
            public IBuildable WithJsonContent(string content)
            {
                Content = Body.For(Body.BodyType.JSON);
                Content.Add("json", content);
                return this;
            }

            /// <inheritdoc />
            public IBuildable WithJson<T>(T item)
            {
                Content = Body.For(Body.BodyType.JSON);
                Content.Add("json", JsonConvert.SerializeObject(item, Formatting.Indented));
                return this;
            }

            /// <inheritdoc />
            public IBuildable MatchExactJsonContent(string content)
            {
                Content = Body.For(Body.BodyType.JSON);
                Content.Add("json", content);
                Content.Add("matchType", "STRICT");
                return this;
            }

            /// <inheritdoc />
            public IBuildable WithXmlContent(string content)
            {
                Content = Body.For(Body.BodyType.XML);
                Content.Add("xml", content);
                return this;
            }

            /// <inheritdoc />
            public IBuildable WithBinaryContent(byte[] content)
            {
                Content = Body.For(Body.BodyType.BINARY);
                Content.Add("binary", content);
                return this;
            }

            /// <inheritdoc />
            public IBuildable WithJsonArray<T>(params T[] items)
            {
                Content = Body.For(Body.BodyType.JSON);
                Content.Add("json", JsonConvert.SerializeObject(items, Formatting.Indented));
                return this;
            }

            /// <inheritdoc />
            public IBuildable EnableEncryption(bool useSsl = true)
            {
                Secure = useSsl;
                return this;
            }

            /// <inheritdoc />
            public IBuildable KeepConnectionAlive(bool keepAlive = true)
            {
                KeepAlive = keepAlive;
                return this;
            }

            /// <inheritdoc />
            public HttpRequest Build()
            {
                return new HttpRequest
                {
                    Body = Content,
                    Cookies = Cookies,
                    Headers = Headers,
                    KeepAlive = KeepAlive,
                    Method = Method,
                    Path = Path,
                    Secure = Secure
                };
            }

            /// <inheritdoc />
            public IBuildable WithHeader(string name, object value)
            {
                Headers ??= new Dictionary<string, object>();
                Headers.TryAdd(name, value);
                return this;
            }

            /// <inheritdoc />
            public IBuildable WithCookie(string name, string value)
            {
                Cookies ??= new Dictionary<string, string>();
                Cookies.TryAdd(name, value);
                return this;
            }

            /// <inheritdoc />
            public IBuildable WithPath(string path)
            {
                Path = path;
                return this;
            }

        }
    }
}