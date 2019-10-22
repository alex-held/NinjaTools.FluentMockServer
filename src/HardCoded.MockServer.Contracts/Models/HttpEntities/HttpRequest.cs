using System;
using System.Collections.Generic;
using System.Net.Http;

using HardCoded.MockServer.Contracts.Abstractions;


namespace HardCoded.MockServer.Contracts.Models.HttpEntities
{
    
    /// <summary>
    /// Model to describe, which request should be matched.
    /// </summary>
    public class HttpRequest : BuildableBase, IEquatable<HttpRequest>
    {
        /// <summary>
        /// The <see cref="HttpMethod"/> to be matched.
        /// </summary>
        public HttpMethod Method { get; set; }
        
        /// <summary>
        /// Header constraints that need to be fulfilled.
        /// </summary>
        public Dictionary<string, object> Headers { get; set; }
        
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
    }
}
