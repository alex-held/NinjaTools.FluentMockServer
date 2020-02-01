using System;
using System.Collections.Generic;
using System.Net;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Models.ValueTypes;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace NinjaTools.FluentMockServer.Models.HttpEntities
{
  
    /// <summary>
    ///     Model to describe how to respond to a matching <see cref="HttpRequest" />.
    /// </summary>
    [PublicAPI]
    public class HttpResponse
    {
        public HttpResponse() { }

        public HttpResponse(int? statusCode, [CanBeNull] Delay delay, [CanBeNull] ConnectionOptions connectionOptions, [CanBeNull] JToken body, [CanBeNull] Dictionary<string, string[]> headers)
        {
            StatusCode = statusCode;
            Delay = delay;
            ConnectionOptions = connectionOptions;
            Body = body;
            Headers = headers;
        }

        /// <summary>
        ///     The <see cref="HttpStatusCode" /> of the <see cref="HttpResponse" />.
        /// </summary>
        public int? StatusCode { get;  set; }

        /// <summary>
        ///     A <see cref="Delay" /> to wait until the <see cref="HttpResponse" /> is returned.
        /// </summary>
        public Delay Delay { get; set; }

        /// <summary>
        ///     Some switches regarding the HttpConnection.
        /// </summary>
        public ConnectionOptions ConnectionOptions { get;  set; }

        public JToken Body { get; set; }

        public Dictionary<string, string[]> Headers { get;  set; }
    }
}
