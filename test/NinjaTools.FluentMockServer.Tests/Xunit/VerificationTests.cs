using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NinjaTools.FluentMockServer.Exceptions;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Xunit;
using NinjaTools.FluentMockServer.Xunit.Attributes;
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


        [Fact]
        [IsolatedMockServerSetup]
        public void VerifyAsync_Should_Return_True_When_MockServer_Recieved_In_Context()
        {
            // Act
          // MockClient.SetupAsync()
            Func<Task> action = async () => await MockClient.VerifyAsync(v => v.WithPath("test"), VerificationTimes.Once);

            // Assert
            action.Should().ThrowExactly<MockServerVerificationException>();
        }
    }
}
