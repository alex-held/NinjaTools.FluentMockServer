using System;
using System.Collections.Generic;
using System.Net;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Models.HttpEntities
{
  
    /// <summary>
    ///     Model to describe how to respond to a matching <see cref="HttpRequest" />.
    /// </summary>
    public class HttpResponse : IEquatable<HttpResponse>
    {
        /// <inheritdoc />
        public bool Equals(HttpResponse other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return StatusCode == other.StatusCode && Equals(Delay, other.Delay) && Equals(ConnectionOptions, other.ConnectionOptions) && Equals(Body, other.Body) && Equals(Headers, other.Headers);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HttpResponse) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StatusCode.GetHashCode();
                hashCode = (hashCode * 397) ^ (Delay != null ? Delay.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ConnectionOptions != null ? ConnectionOptions.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Body != null ? Body.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Headers != null ? Headers.GetHashCode() : 0);
                return hashCode;
            }
        }
        
        public static HttpResponse Create(int? statusCode = null,
            [CanBeNull] Delay delay = null,
            [CanBeNull] ConnectionOptions connectionOptions = null,
            [CanBeNull] JToken body = null,
            [CanBeNull] Dictionary<string, string[]> headers = null)
        {
            return new HttpResponse(statusCode, delay, connectionOptions, body, headers);
        }

        public HttpResponse(int? statusCode, [CanBeNull] Delay delay, [CanBeNull] ConnectionOptions connectionOptions, [CanBeNull] JToken body, [CanBeNull] Dictionary<string, string[]> headers)
        {
            StatusCode = statusCode;
            Delay = delay;
            ConnectionOptions = connectionOptions;
            Body = body;
            Headers = headers;
        }

        [UsedImplicitly]
        private protected HttpResponse()
        {
        }

        /// <summary>
        ///     The <see cref="HttpStatusCode" /> of the <see cref="HttpResponse" />.
        /// </summary>
        public int? StatusCode { get; protected set; }

        /// <summary>
        ///     A <see cref="Delay" /> to wait until the <see cref="HttpResponse" /> is returned.
        /// </summary>
        public Delay Delay { get; protected set; }

        /// <summary>
        ///     Some switches regarding the HttpConnection.
        /// </summary>
        public ConnectionOptions ConnectionOptions { get; protected set; }

        public JToken Body { get; protected set; }

        public Dictionary<string, string[]> Headers { get; protected set; }
    }
}
