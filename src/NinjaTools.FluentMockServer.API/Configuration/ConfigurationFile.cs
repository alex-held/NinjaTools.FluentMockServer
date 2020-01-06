using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.API.Models;
using YamlDotNet.Serialization;

namespace NinjaTools.FluentMockServer.API.Configuration
{
    [JsonArray(AllowNullItems = false)]
    public class ConfigurationFile : List<Setup>, IConfigurationFile
    {
        /// <inheritdoc />
        public string Serialize(ConfigurationFileType fileType = ConfigurationFileType.Yaml)
        {
            
            return fileType switch
            {
                    ConfigurationFileType.Yaml => SerializeYaml(),
                ConfigurationFileType.Json => SerializeJson(),
                _ => throw new NotSupportedException($"Unknown value '{fileType.ToString()}' for enum {nameof(ConfigurationFileType)}.")
            };
        }

        private string SerializeJson()
        {
            return JArray.FromObject(this).ToString(Formatting.Indented);
        }
        
        [NotNull]
        private string SerializeYaml()
        {
            var serializer = new SerializerBuilder()
                .Build();
            
            var sb = new StringBuilder();
            using var writer = new StringWriter(sb);

            serializer.Serialize(writer, this);

            return sb.ToString();
        }
        
    }
}