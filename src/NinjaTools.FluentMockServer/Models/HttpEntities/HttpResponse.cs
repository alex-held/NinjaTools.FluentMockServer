using System.Collections.Generic;
using System.Net;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Models.ValueTypes;


namespace NinjaTools.FluentMockServer.Models.HttpEntities
{
    /// <summary>
    /// Model to describe how to respond to a matching <see cref="HttpRequest"/>.
    /// </summary>
    public class HttpResponse : BuildableBase
    {
        
        /// <inheritdoc />
        public override JObject SerializeJObject()
        {
            var self = base.SerializeJObject();
            
            if (Body != null)
            {
                var oldBody = self.Property("body");
                oldBody.Remove();
                
                var body = Body.Resolve();
                self.Add(body);
                return self;
            }

            return self;
        }

        public HttpResponse()
        { }
        
        public HttpResponse([CanBeNull] int? statusCode = null)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// The <see cref="HttpStatusCode"/> of the <see cref="HttpResponse"/>.
        /// </summary>
        public int? StatusCode { get; set; }
        
        /// <summary>
        /// A <see cref="Delay"/> to wait until the <see cref="HttpResponse"/> is returned.
        /// </summary>
        public Delay Delay { get; set; }
        
        /// <summary>
        /// Some switches regarding the HttpConnection.
        /// </summary>
        public ConnectionOptions ConnectionOptions { get; set; }
        
        public Content Body { get; set; }
        
        public Dictionary<string, string[]> Headers { get; set; }
    }
}
