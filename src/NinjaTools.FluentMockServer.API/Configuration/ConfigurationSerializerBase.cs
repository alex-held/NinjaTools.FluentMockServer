using System;
using System.IO;
using System.IO.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace NinjaTools.FluentMockServer.API.Configuration
{
    public abstract class ConfigurationSerializerBase
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<ConfigurationSerializerBase> _logger;

        protected ConfigurationSerializerBase(IFileSystem fileSystem, ILogger<ConfigurationSerializerBase> logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }


        [NotNull] protected abstract string FileExtension { get; }

        [NotNull]
        protected string[] GetFiles([NotNull] string directory)
        {
            var files = _fileSystem.Directory.GetFiles(directory, $"*.{FileExtension}", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                _logger.LogDebug($"Found configuration file. Path={file};");
            }

            return files;
        }

        internal string? ReadFile([NotNull] string filePath)
        {
            if (_fileSystem.File.Exists(filePath))
            {
                var content = _fileSystem.File.ReadAllText(filePath);
                _logger.LogTrace($"Read configuration file. Path={filePath}; Content=\n{content}");
                return content;
            }

            _logger.LogError($"File does not exists. Path={filePath};");
            return null;
        }

        [CanBeNull]
        internal T? DeserializeFile<T>([NotNull] string path, Func<string, T> deserialize) where T : class, IConfigurationFile, new()
        {
            if (ReadFile(path) is {} content)
            {
                try
                {
                    var config = deserialize(content);
                    _logger.LogDebug($"Successfully deserialized configuration file. Path={path};Content=\n{config.ToString()}");
                    return config;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, $"Error while deserialized configuration file. Path={path};Message={exception.Message}");
                    return null;
                }
            }
            else
            {
                _logger.LogError( $"Unable to deserialize configuration file because no content could be read. Path={path};");
                return null;
            }
        }
    }
}
