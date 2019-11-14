using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Models.ValueTypes;


namespace NinjaTools.FluentMockServer.Models.HttpEntities
{
    /// <summary>
    /// Model to describe how to respond to a matching <see cref="HttpRequest"/>.
    /// </summary>
    public class HttpResponse : BuildableBase, IEquatable<HttpResponse>
    {
        /// <inheritdoc />
        public override JObject SerializeJObject()
        {
            var self = base.SerializeJObject();
            
            if (Body != null)
            {
                var body = Body.ToJProperty();
                self.Add(body);
                return self;
            }

            return self;
        }

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
        
        [JsonIgnore]
        public ResponseBody Body { get; set; }
        
        public Dictionary<string, string[]> Headers { get; set; }
        
        #region Equality Members

        
        /// <inheritdoc />
        public bool Equals(HttpResponse other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return StatusCode == other.StatusCode && Equals(Delay, other.Delay) && Equals(ConnectionOptions, other.ConnectionOptions);
        }


        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals(( HttpResponse ) obj);
        }


        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked {
                var hashCode = StatusCode ?? int.MaxValue;
                hashCode = ( hashCode * 397 ) ^ ( Delay             != null ? Delay.GetHashCode() : 0 );
                hashCode = ( hashCode * 397 ) ^ ( ConnectionOptions != null ? ConnectionOptions.GetHashCode() : 0 );
                return hashCode;
            }
        }


        public static bool operator ==(HttpResponse left, HttpResponse right) => Equals(left, right);


        public static bool operator !=(HttpResponse left, HttpResponse right) => !Equals(left, right);


        #endregion
        
    }
    
    public class ResponseBody
    {
        public ResponseBody(bool isBinary, string content)
        {
            IsBinary = isBinary;
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }
        
        public JProperty ToJProperty()
        {
            if (!IsBinary)
            {
               return new JProperty("body", Content);
            }
            
            return new JProperty("body", new JObject
            {
                ["base64Bytes"] = Content,
                ["type"] = "BINARY"
            });
        }
        
        public bool IsBinary { get; }
        
        public string Content { get; }
    }
}
