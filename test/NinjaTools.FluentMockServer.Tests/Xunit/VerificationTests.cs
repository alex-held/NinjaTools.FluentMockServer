using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using NinjaTools.FluentMockServer.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Xunit
{
    public class VerificationTests : MockServerTestBase
    {
        /// <inheritdoc />
        public VerificationTests(MockServerFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        [Fact]
        public async Task VerifyAsync_Should_Return_True_When_MockServer_Recieved_Matching_Setup()
        {
            // Arrange
            await HttpClient.GetAsync("test");

            // Act
            var (isValid, responseMessage) = await MockClient.VerifyAsync(v => v
                .Verify(b => b.WithPath("test"))
                .Once());

            // Assert
            Output.WriteLine(responseMessage);
            isValid.Should().BeTrue();
            responseMessage.Should().BeEmpty();
        }

        [Fact]
        public async Task VerifyAsync_Should_Return_False_When_MockServer_Recieved_No_Matching_Setup()
        {
            // Act
            var (isValid, responseMessage) = await MockClient.VerifyAsync(v => v
                .Verify(b => b.WithPath("test"))
                .Once());

            // Assert
            Output.WriteLine(responseMessage);
            isValid.Should().BeFalse();
            responseMessage.Should().NotBeNullOrWhiteSpace();
        }
    }

    public abstract class MockServerTestBase : XUnitTestBase, IClassFixture<MockServerFixture>
    {
        public MockServerFixture Fixture { get; }

        public MockServerClient MockClient => Fixture.MockClient;
        public HttpClient HttpClient => MockClient.HttpClient;

        /// <inheritdoc />
        protected MockServerTestBase(MockServerFixture fixture, ITestOutputHelper output) : base(output)
        {
            Fixture = fixture;
        }
    }
}
