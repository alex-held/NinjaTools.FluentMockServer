using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NinjaTools.FluentMockServer.API.Helper;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.API.Models.Logging;
using NinjaTools.FluentMockServer.API.Services;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.API.Tests.Services
{
    public class LogServiceTests : XUnitTestBase<LogServiceTests>
    {
        /// <inheritdoc />
        public LogServiceTests(ITestOutputHelper output) : base(output)
        {
        }

        private ILogService CreateSubject(out Mock<ILogRepository> repo, GenerateId idGenerator = null)
        {
            repo = new Mock<ILogRepository>();

            if (idGenerator != null)
            {
                return new LogService(CreateLogger<LogService>(),repo.Object ,new LogFactory(idGenerator));
            }

            return new LogService(CreateLogger<LogService>(), repo.Object);
        }

        public const string Id = "1234";

        [Fact]
        public void Log_Should_Create_LogItem()
        {
            // Arrange
            var sut = CreateSubject(out var repo,() => Id);
            var setup = new Setup();

            // Act
            sut.Log(l => l.SetupCreated(setup));

            // Assert
            repo.Verify(m => m.AddOrUpdate(It.Is<SetupLog>(l => l.Content == setup)));
        }

        private ILogService CreateSeededLogService()
        {
            var context = new DefaultHttpContext
            {
                Request = { Method = "POST", Path = "/another/path"}
            };
            var sut = CreateSubject(out var repo);
            sut.Log(l => l.SetupCreated(new Setup
            {
                Matcher = new RequestMatcher {Path = "/path"}
            }));
            sut.Log(l => l.SetupDeleted(new Setup
            {
                Matcher = new RequestMatcher {Method = "POST"}
            }));
            sut.Log(l => l.RequestUnmatched(context));
            sut.Log(l => l.RequestMached(context, new Setup
            {
                Matcher = new RequestMatcher {Method = "POST"}
            }));

            return sut;
        }

        [Fact]
        public void OfType_Returns_Only_LogItems_Of_Given_Type()
        {
            // Arrange
            var sut = CreateSeededLogService();

            // Act & Assert
            sut.OfType<SetupLog>().Should().HaveCount(2);
            sut.OfType(LogType.Setup).Should().HaveCount(2);
            sut.OfType(LogType.Request).Should().HaveCount(2);
        }


        [Fact]
        public void Get_Returns_All_Log_Items()
        {
            // Arrange
            var sut = CreateSeededLogService();

            // Act & Assert
            sut.Get().Should().HaveCount(4);
        }


        [Fact]
        public void Prune_Clears_All_Logs()
        {
            // Arrange
            var sut = CreateSeededLogService();

            // Act & Assert
            sut.Prune();
            sut.Get().Should().BeEmpty();
        }


        [Fact]
        public void Get_Returns_Only_LogItems_Matching_The_Predicate()
        {
            // Arrange
            var sut = CreateSeededLogService();

            // Act & Assert
            sut.Get<SetupLog>(log => log.Kind == LogKind.SetupCreated)
                .Should().ContainSingle();
            sut.Get<SetupLog>(log => log.Kind == LogKind.SetupDeleted)
                .Should().ContainSingle();
        }
    }
}
