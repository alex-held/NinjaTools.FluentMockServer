using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.API.Models;
using YamlDotNet.Serialization;

namespace NinjaTools.FluentMockServer.API.Configuration
{
   public class ConfigFileProvider
    {
        private readonly IFileSystem _fs;
        private readonly ILogger<ConfigFileProvider> _logger;

        public ConfigFileProvider(IFileSystem fs, ILogger<ConfigFileProvider> logger)
        {
            _fs = fs;
            _logger = logger;
        }

        private  ConfigFile ParseJson(string path)
        {
            var jo = JArray.Parse(_fs.File.ReadAllText(path));
            var setups = jo.ToObject<List<Setup>>();

            return new ConfigFile(path, setups.ToArray());
        }

        private  IConfigFile ParseYaml(string path)
        {
            using var reader = new StringReader(_fs.File.ReadAllText(path));
            var deserializer = new DeserializerBuilder().Build();

            var setups = deserializer.Deserialize<List<Setup>>(reader);
            return new ConfigFile(path, setups.ToArray());
        }

        public IEnumerable<IConfigFile> GetConfigFiles(string rootDirectory)
        {
            var files = _fs.Directory.GetFiles(rootDirectory, "*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var fileExtension = _fs.Path.GetExtension(file).TrimStart('.').ToLower();

                if (Enum.TryParse<ConfigurationFileType>(fileExtension, out var fileType))
                {
                    foreach (var configFile in ParseConfigFile(fileType, file))
                        yield return configFile;
                }
                else
                {
                    _logger.LogError($"The file extension {fileExtension} is not supported yet.");
                }
            }
        }

        private IEnumerable<IConfigFile> ParseConfigFile(ConfigurationFileType fileType, string file)
        {
            switch (fileType)
            {
                case ConfigurationFileType.yaml:
                    yield return ParseYaml(file);
                    break;
                case ConfigurationFileType.json:
                    yield return ParseJson(file);
                    break;
            }
        }
    }
}
