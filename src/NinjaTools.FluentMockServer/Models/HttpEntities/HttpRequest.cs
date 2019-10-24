using System;
using System.Collections.Generic;
using System.Net.Http;

using Newtonsoft.Json;

using NinjaTools.FluentMockServer.Abstractions;


namespace NinjaTools.FluentMockServer.Models.HttpEntities
{
    
    /// <summary>
    /// Model to describe, which request should be matched.
    /// </summary>
    public class HttpRequest : BuildableBase, IEquatable<HttpRequest>
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
        
        
        
        #region Equality Members

        /// <inheritdoc />
        public bool Equals(HttpRequest other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Equals(HttpMethod, other.HttpMethod) && Equals(Headers, other.Headers) && Equals(Cookies, other.Cookies) && Equals(Body, other.Body) && string.Equals(Path, other.Path, StringComparison.InvariantCultureIgnoreCase) && Secure == other.Secure && KeepAlive == other.KeepAlive;
        }


        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals(( HttpRequest ) obj);
        }


        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked {
                var hashCode = ( HttpMethod != null ? HttpMethod.GetHashCode() : 0 );
                hashCode = ( hashCode * 397 ) ^ ( Headers != null ? Headers.GetHashCode() : 0 );
                hashCode = ( hashCode * 397 ) ^ ( Cookies != null ? Cookies.GetHashCode() : 0 );
                hashCode = ( hashCode * 397 ) ^ ( Body    != null ? Body.GetHashCode() : 0 );
                hashCode = ( hashCode * 397 ) ^ ( Path    != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Path) : 0 );
                hashCode = ( hashCode * 397 ) ^ Secure.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ KeepAlive.GetHashCode();

                return hashCode;
            }
        }


        public static bool operator ==(HttpRequest left, HttpRequest right) => Equals(left, right);


        public static bool operator !=(HttpRequest left, HttpRequest right) => !Equals(left, right);

        #endregion
    }
}
