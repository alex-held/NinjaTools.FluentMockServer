using System;

namespace NinjaTools.FluentMockServer.Client.Models.HttpEntities
{
    public partial class HttpError : IEquatable<HttpError>
    {
        /// <inheritdoc />
        public bool Equals(HttpError other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Delay, other.Delay) && DropConnection == other.DropConnection && ResponseBytes == other.ResponseBytes;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HttpError) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Delay != null ? Delay.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ DropConnection.GetHashCode();
                hashCode = (hashCode * 397) ^ (ResponseBytes != null ? ResponseBytes.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
