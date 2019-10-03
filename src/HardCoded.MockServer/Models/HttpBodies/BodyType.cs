using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HardCoded.MockServer.Models.HttpBodies
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
}