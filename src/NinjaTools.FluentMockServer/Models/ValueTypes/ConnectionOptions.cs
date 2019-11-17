using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Models.HttpEntities;


namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    /// <summary>
    /// Some options regarding a Connection.
    /// </summary>
    [JsonObject(IsReference = true)]
    public class ConnectionOptions : IBuildable
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


        /// <inheritdoc />
        public JObject SerializeJObject()
        {
            var self = new JObject();

            if (KeepAliveOverride.HasValue)
            {
                self.Add("keepAliveOverride", KeepAliveOverride.Value);
            }

            if (SuppressConnectionHeader.HasValue)
            {
                self.Add("suppressConnectionHeader", SuppressConnectionHeader.Value);
            }

            if (SuppressContentLengthHeader.HasValue)
            {
                self.Add("suppressContentLengthHeader", SuppressContentLengthHeader.Value);
            }

            if (ContentLengthHeaderOverride.HasValue)
            {
                self.Add("contentLengthHeaderOverride", ContentLengthHeaderOverride.Value);
            }

            if (CloseSocket.HasValue)
            {
                self.Add("closeSocket", CloseSocket.Value);
            }

            return self;
        }
    }
}
