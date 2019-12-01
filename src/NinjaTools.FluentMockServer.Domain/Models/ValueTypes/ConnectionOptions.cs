using System;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;

namespace NinjaTools.FluentMockServer.Domain.Models.ValueTypes
{
    /// <summary>
    ///     Some options regarding a Connection.
    /// </summary>
    public class ConnectionOptions :IIdentifiable<ConnectionOptions>
    {
        /// <inheritdoc />
        public int Id { get; }

        /// <inheritdoc />
        public DateTime CreatedOn { get; set; }

        /// <inheritdoc />
        public DateTime ModifiedOn{ get; set; }

        /// <inheritdoc />
        public byte[] Timestamp{ get; set; }
        
        /// <summary>
        ///     Whether the MockServer should close the socket after the connection.
        /// </summary>
        public bool? CloseSocket { get; set; }

        /// <summary>
        ///     Overrides the ContentLength Header.
        /// </summary>
        public long? ContentLengthHeaderOverride { get; set; }

        /// <summary>
        ///     Disables the ContentLengthHeadeer
        /// </summary>
        public bool? SuppressContentLengthHeader { get; set; }

        /// <summary>
        ///     Whether to suppress the connection header.
        /// </summary>
        public bool? SuppressConnectionHeader { get; set; }

        /// <summary>
        ///     Overrides the <see cref="HttpRequest.KeepAlive" /> setting.
        /// </summary>
        public bool? KeepAliveOverride { get; set; }

        /// <inheritdoc />
        public bool Equals(ConnectionOptions other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return CloseSocket == other.CloseSocket && ContentLengthHeaderOverride == other.ContentLengthHeaderOverride && SuppressContentLengthHeader == other.SuppressContentLengthHeader && SuppressConnectionHeader == other.SuppressConnectionHeader && KeepAliveOverride == other.KeepAliveOverride;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConnectionOptions) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = CloseSocket.GetHashCode();
                hashCode = (hashCode * 397) ^ ContentLengthHeaderOverride.GetHashCode();
                hashCode = (hashCode * 397) ^ SuppressContentLengthHeader.GetHashCode();
                hashCode = (hashCode * 397) ^ SuppressConnectionHeader.GetHashCode();
                hashCode = (hashCode * 397) ^ KeepAliveOverride.GetHashCode();
                return hashCode;
            }
        }
    }
}
