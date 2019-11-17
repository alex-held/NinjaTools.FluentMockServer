using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Utils;


namespace NinjaTools.FluentMockServer.Models.HttpEntities
{
    /// <summary>
    /// Model used to describe what to verify.
    /// </summary>
    [JsonObject(IsReference = true)]
    public class Verify : IBuildable
    {
        /// <summary>
        /// The to be matched <see cref="HttpRequest"/>.
        /// </summary>
        public HttpRequest HttpRequest { get; set; }

        /// <summary>
        /// How many <see cref="Times"/> the request is expected to have occured.
        /// </summary>
        public VerficationTimes Times { get; set; }


        /// <inheritdoc />
        public JObject SerializeJObject()
        {
            return JObject.FromObject(this, Serializer.Default);

            var self = JObject.FromObject(this, Serializer.Default);

            if (HttpRequest != null)
            {
                self.Add("httpRequest", HttpRequest.SerializeJObject());
            }

            return self;
        }
    }
}
