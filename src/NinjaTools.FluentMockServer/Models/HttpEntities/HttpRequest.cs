using System;
using System.Collections.Generic;
using System.Net.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Utils;


namespace NinjaTools.FluentMockServer.Models.HttpEntities
{

    /// <summary>
    /// Model to describe, which request should be matched.
    /// </summary>
    [JsonObject(IsReference = true)]
    public class HttpRequest : IBuildable
    {
        /// <summary>
        /// The <see cref="System.Net.Http.HttpMethod"/> to be matched.
        /// </summary>
        [JsonIgnore]
        internal HttpMethod HttpMethod
        {
            get => new HttpMethod(Method);
            set => Method = value.Method;
        }


        /// <summary>
        /// The <see cref="System.Net.Http.HttpMethod"/> to be matched.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Header constraints that need to be fulfilled.
        /// </summary>
        public Dictionary<string, string[]> Headers { get; set; }

        /// <summary>
        /// Cookie constraints that need to be fulfilled.
        /// </summary>
        public Dictionary<string, string> Cookies { get; set; }

        /// <summary>
        /// Body constraints that need be fulfilled.
        /// </summary>
        public RequestBody? Body { get; set; }

        /// <summary>
        /// Constrains on the path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Constraint on whether encryption is enabled for this request.
        /// </summary>
        public bool? Secure { get; set; }

        /// <summary>
        /// Constraint on whether to keep the connection alive
        /// </summary>
        public bool? KeepAlive { get; set; }


        /// <inheritdoc />
        public JObject SerializeJObject()
        {
            return JObject.FromObject(this, Serializer.Default);
        }
    }
}
