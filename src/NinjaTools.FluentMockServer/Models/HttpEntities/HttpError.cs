using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Utils;


namespace NinjaTools.FluentMockServer.Models.HttpEntities
{
    /// <summary>
    /// Model to configure an Error.
    /// </summary>
    [JsonObject(IsReference = true)]
    public class HttpError : IBuildable
    {

        /// <inheritdoc />
        public JObject SerializeJObject()
        {
            return JObject.FromObject(this, Serializer.Default);

            var self = new JObject();
            if (DropConnection != null)
            {
                self.Add("dropConnection", true);
            }

            if (string.IsNullOrWhiteSpace(ResponseBytes))
            {
                self.Add("responseBytes", ResponseBytes);
            }

            return self;
        }

        /// <summary>
        /// An optional <see cref="Delay"/> until the <see cref="HttpError"/> occurs.
        /// </summary>
        public Delay Delay { get; set; }

        /// <summary>
        /// Whether to drop the connection when erroring.
        /// </summary>
        public bool? DropConnection { get; set; }

        /// <summary>
        /// Base64 encoded byte response. 
        /// </summary>
        public string ResponseBytes { get; set; }

    }
}
