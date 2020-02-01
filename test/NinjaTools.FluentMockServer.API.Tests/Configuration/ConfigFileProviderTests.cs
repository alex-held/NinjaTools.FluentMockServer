using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using FluentAssertions;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.API.Tests.Configuration
{
    public class ConfigurationProviderTests : XUnitTestBase<ConfigurationProviderTests>
    {
        /// <inheritdoc />
        public ConfigurationProviderTests(ITestOutputHelper output) : base(output)
        { }


        [Fact]
        public void Returns_ConfigurationFile_When_Only_One_Yaml_File()
        {
            // Arrange
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { "/etc/mock-server/config/a.yaml", new MockFileData(@"
- Action:
    Response:
      StatusCode: 200
      Body: ""some body \n over \n multiple \nlines""
")},

                 { "/etc/mock-server/config/b.yml", new MockFileData(@"
- Matcher:
    Path: /some/path
    Method: POST
  Action:
    Response:
      StatusCode: 201
- Matcher:
    Path: /
    Headers:
      Content-Type:
        - application/json
        - application/cloudevents+json
      Content-Length:
        - 105
")}});
            var sut = new ConfigFileProvider(fs, CreateLogger<ConfigFileProvider>());

            // Act
            var configurations = sut.GetConfigFiles().ToArray();

            // Assert
            configurations.Count().Should().Be(2);

            configurations[0].Path.Should().Be("/etc/mock-server/config/a.yaml");
            configurations[0].FileType.Should().Be(ConfigurationFileType.yaml);
            configurations[0].Configurations.Should().HaveCount(1);
            var setupA = configurations[0].Configurations.First();
            setupA.Action.Response.StatusCode.Should().Be(200);
            setupA.Matcher.Should().BeNull();

            configurations[1].Path.Should().Be("/etc/mock-server/config/b.yml");
            configurations[1].FileType.Should().Be(ConfigurationFileType.yml);
            configurations[1].Configurations.Should().HaveCount(2);
            var setupB = configurations[1].Configurations.First();
            setupB.Action.Response.StatusCode.Should().Be(201);
            setupB.Matcher.Path.ToPath().Should().Be("/some/path");
            setupB.Matcher.Method.MethodString.Should().Be("POST");


            var setupC = configurations[1].Configurations.ElementAt(1);
              Dump(setupC, "Setup - C");
            setupC.Matcher.Headers.Header.Should().HaveCount(2);
            setupC.Matcher.Path.ToPath().Should().Be("/");
            setupC.Action.Should().BeNull();
        }



        [Fact]
        public void Returns_ConfigurationFile_When_Only_Only_JSON_Files()
        {
            // Arrange
            var fs = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { "/etc/mock-server/config/a.json", new MockFileData(@"
[
  {
    ""Action"": {
      ""Response"": {
        ""StatusCode"": 200,
        ""Body"": ""some body \n over \n multiple \nlines""
      }
    }
  }
]
")},

                 { "/etc/mock-server/config/b.json", new MockFileData(@"
[
  {
    ""Matcher"": {
      ""Path"": ""/some/path"",
      ""Method"": ""POST""
    },
    ""Action"": {
      ""Response"": {
        ""StatusCode"": 201
      }
    }
  },
  {
    ""Matcher"": {
      ""Path"": ""/"",
      ""Headers"": {
        ""Content-Type"": [
          ""application/json"",
          ""application/cloudevents+json""
        ],
        ""Content-Length"": [
          105
        ]
      }
    }
  }
]
")}});
            var sut = new ConfigFileProvider(fs, CreateLogger<ConfigFileProvider>());

            // Act
            var configurations = sut.GetConfigFiles().ToArray();

            // Assert
            configurations.Count().Should().Be(2);

            configurations[0].Path.Should().Be("/etc/mock-server/config/a.json");
            configurations[0].FileType.Should().Be(ConfigurationFileType.json);
            configurations[0].Configurations.Should().HaveCount(1);
            var setupA = configurations[0].Configurations.First();
            setupA.Action.Response.StatusCode.Should().Be(200);
            setupA.Matcher.Should().BeNull();

            configurations[1].Path.Should().Be("/etc/mock-server/config/b.json");
            configurations[1].FileType.Should().Be(ConfigurationFileType.json);
            configurations[1].Configurations.Should().HaveCount(2);
            var setupB = configurations[1].Configurations.First();
            setupB.Action.Response.StatusCode.Should().Be(201);
            setupB.Matcher.Path.ToPath().Should().Be("/some/path");
            setupB.Matcher.Method.MethodString.Should().Be("POST");


            var setupC = configurations[1].Configurations.ElementAt(1);
            setupC.Matcher.Headers.Header.Should().HaveCount(2);
            setupC.Matcher.Path.ToPath().Should().Be("/");
            setupC.Action.Should().BeNull();
        }


        public static IEnumerable<object[]> GetFakeFileSystemConfigData()
        {
            var directory = "/etc/mock-server/config";

            yield return new object[]
            {
                new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    {
                        $"{directory}/setup.yaml", new MockFileData(@"
  Matcher:
    BodyMatcher:
      Content: ""request body to match \n over \n multiple \nlines""
      Type: Text
      MatchExact: false
    Path: /some/path")
                    }
                }),
                1,
                directory
            };
        }
    }
}
