using System;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Models.HttpEntities;

namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    /// <summary>
    ///     Some options regarding a Connection.
    /// </summary>
    [PublicAPI]
    public class ConnectionOptions
    {
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
        ///     Disables the ContentLengthHeader
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
    }
}
