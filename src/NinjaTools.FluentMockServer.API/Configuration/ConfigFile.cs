using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.API.Models;
using YamlDotNet.Serialization;

namespace NinjaTools.FluentMockServer.API.Configuration
{
    [DebuggerDisplay("{DebuggerDisplay()}")]
    public class ConfigFile : IConfigFile
    {
        [JsonConstructor]
        public ConfigFile([NotNull] string path, [NotNull] params Setup[] setups)
        {
            Path = path;

            var fileExtension = System.IO.Path.GetExtension(Path).TrimStart('.').ToLower(CultureInfo.InvariantCulture);
            if (Enum.TryParse<ConfigurationFileType>(fileExtension, out var fileType))
            {
                FileType = fileType;
            }

            Configurations = new ConfigurationCollection(setups);
        }

        [JsonIgnore]
        [YamlIgnore]
        public string Path { get; }

        [JsonIgnore]
        [YamlIgnore]
        public ConfigurationFileType? FileType { get; }

        [NotNull]
        [Required]
        [JsonRequired]
        public ConfigurationCollection Configurations { get; }

        [NotNull]
        private string DebuggerDisplay()
        {
            var count = (this as IConfigFile).Count;
            var fileType = FileType.HasValue ? FileType.ToString() : "Unkown filetype!";
            return $"Count: {count.ToString()}; Path={Path}; FileType={fileType}";
        }
    }
}
