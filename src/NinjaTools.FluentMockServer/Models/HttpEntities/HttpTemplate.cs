using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Utils;


namespace NinjaTools.FluentMockServer.Models.HttpEntities
{

    [JsonObject(IsReference = true)]
    public class HttpTemplate : IBuildable
    {
        public string Template { get; set; }

        public Delay Delay { get; set; }



        /// <inheritdoc />
        public JObject SerializeJObject()
        {
            return JObject.FromObject(this, Serializer.Default);
        }
    }
}
