using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Domain.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Domain.Models.HttpEntities
{
    /// <summary>
    ///     Model to describe, which request should be matched.
    /// </summary>
    public partial class HttpRequest
    {
        private HttpRequest()
        {
        }

        public HttpRequest(string method, Dictionary<string, string[]> headers, Dictionary<string, string> cookies, JToken body, string path, bool? secure, bool? keepAlive)
        {
            Method = method;
            Headers = headers;
            Cookies = cookies;
            Body = body;
            Path = path;
            Secure = secure;
            KeepAlive = keepAlive;
        }

        /// <summary>
        ///     The <see cref="System.Net.Http.HttpMethod" /> to be matched.
        /// </summary>
        public string Method { get; }

        /// <summary>
        ///     Header constraints that need to be fulfilled.
        /// </summary>
        public Dictionary<string, string[]> Headers { get; }

        /// <summary>
        ///     Cookie constraints that need to be fulfilled.
        /// </summary>
        public Dictionary<string, string> Cookies { get; }

        /// <summary>
        ///     Body constraints that need be fulfilled.
        /// </summary>
        public JToken Body { get; }

        /// <summary>
        ///     Constrains on the path
        /// </summary>
        public string Path { get; }

        /// <summary>
        ///     Constraint on whether encryption is enabled for this request.
        /// </summary>
        public bool? Secure { get; }

        /// <summary>
        ///     Constraint on whether to keep the connection alive
        /// </summary>
        public bool? KeepAlive { get; }

        [NotNull]
        public static HttpRequest Create(
            [CanBeNull] string method = null,
            [CanBeNull] string path = null,
            [CanBeNull] Body body = null,
            [CanBeNull] Dictionary<string, string[]> headers = null,
            [CanBeNull] Dictionary<string, string> cookies = null,
            bool? secure = null,
            bool? keepAlive = null)
        {
            return new HttpRequest(method, headers, cookies, body, path, secure, keepAlive);
        }
    }
}
