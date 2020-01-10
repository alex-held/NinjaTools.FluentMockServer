using JetBrains.Annotations;
using YamlDotNet.Serialization;

namespace NinjaTools.FluentMockServer.API.Configuration
{
    public interface IConfigFile
    {
        [YamlIgnore]
        public int Count => Configurations.Count;

        [NotNull]
        [YamlIgnore]
        string Path { get; }

        /// <summary>
        /// The <see cref="FileType"/> will only be null when we don't support it.
        /// </summary>
        [CanBeNull]
        [YamlIgnore]
        public ConfigurationFileType? FileType { get; }

        [NotNull]
        ConfigurationCollection Configurations { get; }
    }
}