using System;
using System.Net;

using HardCoded.MockServer.Contracts.Abstractions;
using HardCoded.MockServer.Contracts.Models.ValueTypes;


namespace HardCoded.MockServer.Contracts.Models.HttpEntities
{
    /// <summary>
    /// Model to describe how to respond to a matching <see cref="HttpRequest"/>.
    /// </summary>
    public class HttpResponse : BuildableBase, IEquatable<HttpResponse>
    {
        public HttpResponse(int statusCode)
        {
            StatusCode = statusCode;
            ConnectionOptions = new ConnectionOptions();
            Delay = Delay.None;
        }
        
        /// <summary>
        /// The <see cref="HttpStatusCode"/> of the <see cref="HttpResponse"/>.
        /// </summary>
        public int StatusCode { get; set; }
        
        /// <summary>
        /// A <see cref="Delay"/> to wait until the <see cref="HttpResponse"/> is returned.
        /// </summary>
        public Delay Delay { get; set; }
        
        /// <summary>
        /// Some switches regarding the HttpConnection.
        /// </summary>
        public ConnectionOptions ConnectionOptions { get; set; }
        
        /// <summary>
        /// The Content of the <see cref="HttpResponse"/>.
        /// </summary>
        public RequestBody Body { get; set; }
    }
}
