using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NinjaTools.FluentMockServer.Exceptions;
using NinjaTools.FluentMockServer.Models.ValueTypes;
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
            await MockClient.VerifyAsync(v => v.WithPath("test"), VerificationTimes.Once);
        }

        [Fact]
        public void VerifyAsync_Should_Return_False_When_MockServer_Recieved_No_Matching_Setup()
        {
            // Act
            Func<Task> action = async () => await MockClient.VerifyAsync(v => v.WithPath("test"), VerificationTimes.Once);

            // Assert
            action.Should().ThrowExactly<MockServerVerificationException>();
        }
    }

    [CollectionDefinition(nameof(MockServerCollectionFixture), DisableParallelization = true)]
    public class MockServerCollectionFixture : ICollectionFixture<MockServerFixture>
    { }

    [Collection(nameof(MockServerCollectionFixture))]
    public abstract class MockServerTestBase : XUnitTestBase, IDisposable
    {
        public MockServerFixture Fixture { get; }

        public MockServerClient MockClient => Fixture.MockClient;
        public HttpClient HttpClient => MockClient.HttpClient;

        /// <inheritdoc />
        protected MockServerTestBase(MockServerFixture fixture, ITestOutputHelper output) : base(output)
        {
            Fixture = fixture;
            Thread.Sleep(200);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            MockClient.ResetAsync().GetAwaiter().GetResult();
        }
    }
}
