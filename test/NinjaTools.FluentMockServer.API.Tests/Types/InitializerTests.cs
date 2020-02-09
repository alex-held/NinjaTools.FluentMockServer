using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using NinjaTools.FluentMockServer.API.Administration;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.API.Logging;
using NinjaTools.FluentMockServer.API.Types;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.API.Tests.Types
{
    public class InitializerTests : XUnitTestBase<InitializerTests>
    {
        /// <inheritdoc />
        public InitializerTests(ITestOutputHelper output) : base(output)
        { }

        [Fact]
        public async Task Should_Initialize_StartupInitializer_On_Startup()
        {
            // Arrange
            var mockInitializer = new Mock<IInitializer>();
            var opt = new StartupInitializerOptions
            {
                EnableConfigInitializer = true,
                EnableLoggingInitializer = true
            };

            var startupInitializer = new StartupInitializer(CreateLogger<StartupInitializer>(), opt);

            // Act
            startupInitializer.AddInitializer(mockInitializer.Object);
            await startupInitializer.InitializeAsync();

            // Assert
            mockInitializer.Verify(m => m.InitializeAsync(), Times.Once);
        }

        [Fact]
        public async Task Should_Not_Initialize_Turned_Of_Initializers_On_Startup()
        {
            // Arrange
            var loggingInit = new Mock<ILoggingInitializer>();
            var configInit = new Mock<IConfigurationInitializer>();

            var opt = new StartupInitializerOptions
            {
                EnableConfigInitializer = true,
                EnableLoggingInitializer = false
            };

            var startupInitializer = new StartupInitializer(CreateLogger<StartupInitializer>(), opt);

            // Act
            startupInitializer.AddInitializer(loggingInit.Object);
            startupInitializer.AddInitializer(configInit.Object);
            await startupInitializer.InitializeAsync();

            // Assert
            loggingInit.Verify(m => m.InitializeAsync(), Times.Never);
            configInit.Verify(m => m.InitializeAsync(), Times.Once);
        }

        [Fact]
        public async Task ConfigurationInitializer_Should_Initialize_On_Startup()
        {
            // Arrange
            var configService = new Mock<IConfigurationService>();
            var sut = new ConfigurationInitializer(configService.Object);

            // Act
            await sut.InitializeAsync();

            // Assert
            configService.Verify(m => m.Reload());
        }
    }
}
