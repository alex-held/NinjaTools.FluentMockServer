using System;

using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Models.HttpEntities;


namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    /// <summary>
    /// Some options regarding a Connection.
    /// </summary>
    public class ConnectionOptions : BuildableBase, IEquatable<ConnectionOptions>
    {
        /// <summary>
        /// Whether the MockServer should close the socket after the connection.
        /// </summary>
        public bool? CloseSocket { get; set; }

        /// <summary>
        /// Overrides the ContentLength Header.
        /// </summary>
        public long? ContentLengthHeaderOverride { get; set; }
        
        /// <summary>
        /// Disables the ContentLengthHeadeer
        /// </summary>
        public bool? SuppressContentLengthHeader { get; set; }

        /// <summary>
        /// Whether to suppress the connection header.
        /// </summary>
        public bool? SuppressConnectionHeader { get; set; }

        /// <summary>
        /// Overrides the <see cref="HttpRequest.KeepAlive"/> setting.
        /// </summary>
        public bool? KeepAliveOverride { get; set; }


        #region Equality Members

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

            return Equals(( ConnectionOptions ) obj);
        }


        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked {
                var hashCode = CloseSocket.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ ContentLengthHeaderOverride.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ SuppressContentLengthHeader.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ SuppressConnectionHeader.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ KeepAliveOverride.GetHashCode();

                return hashCode;
            }
        }


        public static bool operator ==(ConnectionOptions left, ConnectionOptions right) => Equals(left, right);


        public static bool operator !=(ConnectionOptions left, ConnectionOptions right) => !Equals(left, right);

        #endregion
    }
}
