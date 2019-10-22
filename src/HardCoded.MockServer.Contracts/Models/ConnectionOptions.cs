using System;

using HardCoded.MockServer.Contracts.Abstractions;
using HardCoded.MockServer.Contracts.Models.HttpEntities;


namespace HardCoded.MockServer.Contracts.Models
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


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HardCoded.MockServer.Models
{
    public class ConnectionOptions : BuildableBase
    {
        public bool CloseSocket { get; set; }

        public long ContentLengthHeaderOverride { get; set; }
        
        public bool SuppressContentLengthHeader { get; set; }

        public bool SuppressConnectionHeader { get; set; }

        public bool KeepAliveOverride { get; set; }
    }
}
