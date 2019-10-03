using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HardCoded.MockServer.Models.HttpBodies
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MatchType
    {
        STRICT,
        ONLY_MATCHING_FIELDS
    }
}