using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Moq;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.API.Services;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.API.Tests.Configuration
{
    public class ConfigurationServiceTests : XUnitTestBase<ConfigurationServiceTests>
    {
        /// <inheritdoc />
        public ConfigurationServiceTests(ITestOutputHelper output) : base(output)
        {
        }

        private IConfigurationService CreateSubject(out Mock<IConfigFileProvider> provider, out Mock<ISetupService> setupService)
        {
            var options = Options.Create(new MockServerOptions
            {
                ConfigFilePath = "/var/mock-server/config"
            });

            provider = new Mock<IConfigFileProvider>();
            setupService =  new Mock<ISetupService>();

            return new ConfigurationService(setupService.Object, provider.Object);
        }

        [Fact]
        public void Reload_Should_Get_Configurations()
        {
            // Arrange
            var subject = CreateSubject(out var provider, out var setupService);
            provider.Setup(m => m.GetConfigFiles()).Returns(new List<IConfigFile>
            {
                new ConfigFile("/var/mock-server/config/config.yaml", new[] {new Setup(), new Setup()})
            });

            // Act
            subject.Reload();

            // Assert
            setupService.Verify(m => m.Add(It.IsAny<Setup>()), Times.Exactly(2));
        }
    }
}
