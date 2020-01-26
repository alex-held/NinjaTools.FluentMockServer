using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.API.Logging;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.API.Serialization.Converters;
using YamlDotNet.Serialization;

namespace NinjaTools.FluentMockServer.API.Configuration
{

    public interface IConfigFileProvider
    {
        IEnumerable<IConfigFile> GetConfigFiles();
    }

    public class ConfigFileProvider : IConfigFileProvider
    {
        private readonly IFileSystem _fs;
        private readonly ILogger<ConfigFileProvider> _logger;

        public ConfigFileProvider(IFileSystem fs, ILogger<ConfigFileProvider> logger)
        {
            _fs = fs;
            _logger = logger;
        }

        public ConfigFileProvider(ILogger<ConfigFileProvider> logger) : this(new FileSystem(), logger)
        { }


        public IEnumerable<IConfigFile> GetConfigFiles()
        {
            var rootDirectory = MockServerPaths.Configs;
            var files = _fs.Directory.GetFiles(rootDirectory, "*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var fileExtension = _fs.Path.GetExtension(file).TrimStart('.').ToLower();

                if (Enum.TryParse<ConfigurationFileType>(fileExtension, out var fileType))
                {
                    foreach (var configFile in ParseConfigFile(fileType, file))
                    {
                        yield return configFile;
                    }
                }
                else
                {
                    _logger.LogError($"The file extension {fileExtension} is not supported yet.");
                }
            }
        }

        private ConfigFile ParseJson(string path)
        {
            var text = _fs.File.ReadAllText(path);
            var setups = JsonConvert.DeserializeObject<List<Setup>>(text);
            return new ConfigFile(path, setups.ToArray());
        }

        private IConfigFile ParseYaml(string path)
        {
            using var reader = new StringReader(_fs.File.ReadAllText(path));

            var deserializer = new DeserializerBuilder()
                .IgnoreFields()
                .IgnoreUnmatchedProperties()
                .WithTypeConverter(new MethodConverter())
                .WithTypeConverter(new HeadersConverter())
                .WithTypeConverter(new PathConverter())
                .Build();

            var setups = deserializer.Deserialize<List<Setup>>(reader);
            return new ConfigFile(path, setups.ToArray());
        }


        private IEnumerable<IConfigFile> ParseConfigFile(ConfigurationFileType fileType, string file)
        {
            if (fileType == ConfigurationFileType.yaml)
            {
                yield return ParseYaml(file);
            }
            else if (fileType == ConfigurationFileType.json)
            {
                yield return ParseJson(file);
            }
        }
    }
}
