using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Models.HttpEntities;


namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    /// <summary>
    /// Some options regarding a Connection.
    /// </summary>
        public class ConnectionOptions 
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
    }
}
