using System;

namespace NinjaTools.FluentMockServer.Models.HttpEntities
{
    public partial class Verify : IEquatable<Verify>
    {
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
