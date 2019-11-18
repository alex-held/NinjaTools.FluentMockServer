using System.Collections.Generic;
using Newtonsoft.Json.Linq;


namespace NinjaTools.FluentMockServer.Models.HttpEntities
{

    /// <summary>
    /// Model to describe, which request should be matched.
    /// </summary>
    public class HttpRequest 
    {
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
        public JToken Body { get; set; }

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
