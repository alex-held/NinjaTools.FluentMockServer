using System;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Domain.Builders.Response;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;

namespace NinjaTools.FluentMockServer.Domain.Models.ValueTypes
{
    /// <summary>
    ///     Some options regarding a Connection.
    /// </summary>
    public class ConnectionOptions : IEquatable<ConnectionOptions>
    {
        internal class FluentConnectionOptionsBuilder : IFluentConnectionOptionsBuilder
        {
            private  bool? _closeSocket;
            private  long? _contentLengthHeaderOverride;
            private  bool? _suppressContentLengthHeader;
            private  bool? _suppressConnectionHeader;
            private  bool? _keepAliveOverride;

            /// <inheritdoc />
            public IFluentConnectionOptionsBuilder WithKeepAliveOverride(bool keepAliveOverride)
            {
                _keepAliveOverride = keepAliveOverride;
                return this;
            }

            [NotNull]
            public ConnectionOptions Build()
            {
                return new ConnectionOptions(_closeSocket, _contentLengthHeaderOverride, _suppressContentLengthHeader, _suppressConnectionHeader, _keepAliveOverride);
            }
        }
        
        public ConnectionOptions(bool? closeSocket, long? contentLengthHeaderOverride, bool? suppressContentLengthHeader, bool? suppressConnectionHeader, bool? keepAliveOverride)
        {
            CloseSocket = closeSocket;
            ContentLengthHeaderOverride = contentLengthHeaderOverride;
            SuppressContentLengthHeader = suppressContentLengthHeader;
            SuppressConnectionHeader = suppressConnectionHeader;
            KeepAliveOverride = keepAliveOverride;
        }
        
        /// <summary>
        ///     Whether the MockServer should close the socket after the connection.
        /// </summary>
        public bool? CloseSocket { get; }

        /// <summary>
        ///     Overrides the ContentLength Header.
        /// </summary>
        public long? ContentLengthHeaderOverride { get; }

        /// <summary>
        ///     Disables the ContentLengthHeadeer
        /// </summary>
        public bool? SuppressContentLengthHeader { get; }

        /// <summary>
        ///     Whether to suppress the connection header.
        /// </summary>
        public bool? SuppressConnectionHeader { get; }

        /// <summary>
        ///     Overrides the <see cref="HttpRequest.KeepAlive" /> setting.
        /// </summary>
        public bool? KeepAliveOverride { get; }

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
