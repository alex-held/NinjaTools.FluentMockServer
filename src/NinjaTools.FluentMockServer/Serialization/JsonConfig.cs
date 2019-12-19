using Newtonsoft.Json;

namespace NinjaTools.FluentMockServer.Serialization
{
    /// <inheritdoc />
    public class JsonConfig : JsonSerializerSettings
    {
        public static readonly JsonConfig Instance = new JsonConfig();

        public JsonConfig()
        {
            var resolver = Serialization.ContractResolver.Instance;
            StringEscapeHandling = StringEscapeHandling.EscapeHtml;
            NullValueHandling = NullValueHandling.Ignore;
            Formatting = Formatting.Indented;
            ContractResolver = resolver;
        }
    }
}
