using System;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Xunit;
using Xunit;
using Xunit.Abstractions;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly, DisableTestParallelization = false)]
namespace NinjaTools.FluentMockServer.Tests.Xunit
{
    public class LoggerTests : MockServerTestBase, IDisposable
    {
        /// <inheritdoc />
        public LoggerTests([NotNull] MockServerFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        [Fact]
        public async Task Should_Log_Messages()
        {
            // Act
            await MockClient.ResetAsync();

            // Assert
            var logs = XunitContext.Logs;
            logs.Should().Contain("Resetting MockServer...");
        }

        /// <inheritdoc />
        public void Dispose()
        {
            XunitContext.Flush();
        }
    }
}
