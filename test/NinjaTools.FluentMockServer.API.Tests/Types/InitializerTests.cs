using System.Threading.Tasks;
using Moq;
using NinjaTools.FluentMockServer.API.Configuration;
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
            var startupInitializer = new StartupInitializer(CreateLogger<StartupInitializer>());

            // Act
            startupInitializer.AddInitializer(mockInitializer.Object);
            await startupInitializer.InitializeAsync();

            // Assert
            mockInitializer.Verify(m => m.InitializeAsync(), Times.Once);
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
