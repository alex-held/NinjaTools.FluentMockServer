using System;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Models
{
    /// <summary>
    ///     Model used to describe what to verify.
    /// </summary>
    public class Verify : IEquatable<Verify>
    {
        public Verify(HttpRequest httpRequest, VerificationTimes times)
        {
            Times = times;
            HttpRequest = httpRequest;
        }

        /// <summary>
        ///     The to be matched <see cref="HttpRequest" />.
        /// </summary>
        public HttpRequest HttpRequest { get;  }

        /// <summary>
        ///     How many <see cref="Times" /> the request is expected to have occured.
        /// </summary>
        public VerificationTimes Times { get; }

        public static Verify Once(HttpRequest httpRequest = null)
        {
            return new Verify(httpRequest, VerificationTimes.Once);
        }
        
        /// <inheritdoc />
        public bool Equals(Verify other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(HttpRequest, other.HttpRequest) && Equals(Times, other.Times) && Equals(HttpRequest, other.HttpRequest) && Equals(Times, other.Times);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Verify) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (HttpRequest != null ? HttpRequest.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Times != null ? Times.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (HttpRequest != null ? HttpRequest.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Times != null ? Times.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
