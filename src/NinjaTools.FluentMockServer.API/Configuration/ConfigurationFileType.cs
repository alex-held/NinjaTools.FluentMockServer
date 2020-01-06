using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NinjaTools.FluentMockServer.API.Configuration
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ConfigurationFileType : ushort
    {
        Yaml = 1,
        Json = 2
    }
}