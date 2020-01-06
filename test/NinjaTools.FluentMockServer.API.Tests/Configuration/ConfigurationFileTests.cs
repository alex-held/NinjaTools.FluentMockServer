using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.API.Tests.Configuration
{
    public class ConfigurationFileTests : XUnitTestBase<ConfigurationFileTests>
    {
        /// <inheritdoc />
        public ConfigurationFileTests(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [MemberData(nameof(GetValidConfigurationData), ConfigurationFileType.Yaml)]
        public void Serialize_Returns_Valid_Yaml_When_Instance_Is_Valid([NotNull] ConfigurationFile config, string expected)
        {
            // Arrange
            var expectedSetupCount = config.Count();

            // Act
            var serialized = config.Serialize(ConfigurationFileType.Yaml);
            Dump(serialized, header: "Serialized");

            // Assert
            var (actual, actualSetupCount) = ProcessSerialization(serialized);
            Dump(expected, actual);

            actualSetupCount.Should().Be(expectedSetupCount);
            actual.Should().Be(expected);
        }


        [Theory]
        [MemberData(nameof(GetValidConfigurationData), ConfigurationFileType.Json)]
        public void Serialize_Returns_Valid_Json_When_Instance_Is_Valid([NotNull] ConfigurationFile config, string expected)
        {
            // Arrange
            var expectedSetupCount = config.Count();

            // Act
            var serialized = config.Serialize(ConfigurationFileType.Json);
            Dump(serialized, header: "Serialized");

            // Assert
            var (actual, actualSetupCount) = ProcessJsonSerialization(serialized);
            Dump(expected, actual);

            actualSetupCount.Should().Be(expectedSetupCount);
            actual.Should().Be(expected);
        }

        public static (string actual, int actualSetupCount) ProcessJsonSerialization(string serialized)
        {
            var actualSetupCount = 0;

            // ReSharper disable once HeapView.BoxingAllocation
            var setups = JToken.Parse(serialized);
            var ids = setups.SelectTokens("$.[*].Id");

            foreach (var id in ids)
            {
                id.Replace($"{(++actualSetupCount).ToString()}");
            }


            var actual = setups.ToString(Formatting.Indented);

            return (actual, actualSetupCount);
        }

        protected (string actual, int actualSetupCount) ProcessSerialization(string serialized)
        {
            var actualSetupCount = 0;

            var actual = string.Join('\n', serialized
                    .Split('\n')
                    .Select(line =>
                    {
                        if (!line.StartsWith("- Id:")) return line;
                        actualSetupCount++;
                        return $"- Id: {actualSetupCount.ToString()}";
                    }))
                .TrimEnd('\n');

            return (actual, actualSetupCount);
        }

        [ItemNotNull]
        public static IEnumerable<object[]> GetValidConfigurationData(ConfigurationFileType configurationFileType)
        {
            var expectedIndex = configurationFileType switch
            {
                ConfigurationFileType.Yaml => ValidConfigurationFileData.YamlIndex,
                ConfigurationFileType.Json => ValidConfigurationFileData.JsonIndex,
                _ => throw new NotSupportedException()
            };
            var testData = new ValidConfigurationFileData().Select(data =>
            {
                var config = data[ValidConfigurationFileData.ConfigurationFileIndex];
                var expected = data[expectedIndex];
                return new object[] {config, expected};
            }).ToList();
            return testData;
        }
    }
}
