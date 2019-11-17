using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Utils;


namespace NinjaTools.FluentMockServer.Models.HttpEntities
{
    /// <summary>
    /// Model to describe to which destination the <see cref="HttpRequest"/> to forward.
    /// </summary>
    [JsonObject(IsReference = true)]
    public class HttpForward : IBuildable
    {


        /// <inheritdoc />
        public JObject SerializeJObject()
        {
            return JObject.FromObject(this, Serializer.Default);

            var self = new JObject();
            if (!string.IsNullOrWhiteSpace(Host))
            {
                self.Add("host", Host);
            }
            if (Port.HasValue)
            {
                self.Add("port", Port.Value);
            }

            return self;
        }

        /// <summary>
        /// Gets and sets the Hostname to forward to.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets and sets the Port to forward to.
        /// </summary>
        public int? Port { get; set; }
    }
}
