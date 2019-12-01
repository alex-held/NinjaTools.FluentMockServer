using System;

namespace NinjaTools.FluentMockServer.Domain.Models.HttpEntities
{
    /// <summary>
    ///     Model to describe, which request should be matched.
    /// </summary>
    public partial class HttpRequest : IIdentifiable<HttpRequest>
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public DateTime CreatedOn { get; set; }

        /// <inheritdoc />
        public DateTime ModifiedOn{ get; set; }

        /// <inheritdoc />
        public byte[] Timestamp{ get; set; }
        
        
        /// <inheritdoc />
        public bool Equals(HttpRequest other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Method == other.Method && Equals(Headers, other.Headers) && Equals(Cookies, other.Cookies) && Equals(Body, other.Body) && Path == other.Path && Secure == other.Secure && KeepAlive == other.KeepAlive;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HttpRequest) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Method != null ? Method.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Headers != null ? Headers.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Cookies != null ? Cookies.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Body != null ? Body.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Path != null ? Path.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Secure.GetHashCode();
                hashCode = (hashCode * 397) ^ KeepAlive.GetHashCode();
                return hashCode;
            }
        }


    }
}
