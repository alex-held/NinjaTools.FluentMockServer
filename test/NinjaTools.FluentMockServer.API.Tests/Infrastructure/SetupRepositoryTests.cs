using System;
using Moq;
using NinjaTools.FluentMockServer.API.Helper;
using NinjaTools.FluentMockServer.API.Infrastructure;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.API.Models.Logging;
using NinjaTools.FluentMockServer.API.Services;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.API.Tests.Infrastructure
{
    public class SetupRepositoryTests : XUnitTestBase<SetupRepositoryTests>
    {
        /// <inheritdoc />
        public SetupRepositoryTests(ITestOutputHelper output) : base(output)
        { }

        private ISetupRepository CreateSubject(out Mock<ILogService> logService)
        {
            logService = new Mock<ILogService>();
            return new SetupRepository(logService.Object);
        }

        [Fact]
        public void Add_Should_Create_A_SetupLogCreated_Log_Entry()
        {
            // Arrange
            var sut = CreateSubject(out var logService);
            var setup = new Setup
            {
                Matcher = new RequestMatcher
                {
                    Path = "/some/path"
                }
            };

            // Act
            sut.Add(setup);

            // Assert
            logService.Verify(m => m.Log(It.IsAny<Func<LogFactory, LogItem<Setup>>>()));
        }
    }
}
