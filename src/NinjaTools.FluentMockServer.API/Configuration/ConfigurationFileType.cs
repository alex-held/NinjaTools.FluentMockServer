using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NinjaTools.FluentMockServer.API.Configuration
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ConfigurationFileType
    {
        yaml = 1,
        yml = 1,
        json = 2
    }
}
