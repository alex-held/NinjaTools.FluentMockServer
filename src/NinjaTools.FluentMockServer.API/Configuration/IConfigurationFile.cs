using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NinjaTools.FluentMockServer.API.Configuration
{
    public interface IConfigurationFile
    {
        string ToString() => JObject.FromObject(this).ToString(Formatting.Indented);
        
        string Serialize(ConfigurationFileType fileType = ConfigurationFileType.Yaml);
    }
}