using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Models.HttpEntities
{
    /// <summary>
    ///     Model to describe how to respond to a matching <see cref="HttpRequest" />.
    /// </summary>
    public class HttpResponse
    {
        /// <summary>
        ///     The <see cref="HttpStatusCode" /> of the <see cref="HttpResponse" />.
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        ///     A <see cref="Delay" /> to wait until the <see cref="HttpResponse" /> is returned.
        /// </summary>
        public Delay Delay { get; set; }

        /// <summary>
        ///     Some switches regarding the HttpConnection.
        /// </summary>
        public ConnectionOptions ConnectionOptions { get; set; }

        public JToken Body { get; set; }

        public Dictionary<string, string[]> Headers { get; set; }
    }
}
