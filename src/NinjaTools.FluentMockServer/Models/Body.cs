using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace NinjaTools.FluentMockServer.Models
{
    public class Body : JObject
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BodyType
        {
            JSON_SCHEMA,
            JSON_PATH,
            JSON,
            STRING,
            XML_SCHEMA,
            XML,
            XPATH,
            REGEX,
            BINARY
        }

        internal Body AddOrUpdate(string key, string value)
        {
            return AddOrUpdate(key, new JValue(value));
        }

        public static Body Init(string type)
        {
            var body = new Body {{"type", new JValue(type)}};
            return body;
        }

        public Body AddOrUpdate(string key, JToken token)
        {
            if (TryGetValue(key, out var value)) value.Replace(token);
            Add(key, token);
            return this;
        }
    }
}
