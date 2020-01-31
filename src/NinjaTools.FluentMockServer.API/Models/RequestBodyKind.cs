using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NinjaTools.FluentMockServer.API.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RequestBodyKind
    {
        Text,
        Base64
    }
}
