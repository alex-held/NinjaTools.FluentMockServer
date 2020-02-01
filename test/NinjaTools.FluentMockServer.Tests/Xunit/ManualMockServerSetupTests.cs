using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Xunit
{
    public class ManualMockServerSetupTests : MockServerTestBase
    {
        /// <inheritdoc />
        public ManualMockServerSetupTests(MockServerFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        [Fact]
        public async Task Should_Successfully_Setup_Up_Expecation_On_MockServer()
        {
            // Act
            await MockClient.SetupAsync(exp =>
                exp.OnHandling(HttpMethod.Get, req => req.WithPath("/test"))
                    .RespondOnce(HttpStatusCode.Created, resp => resp.WithDelay(50, TimeUnit.Milliseconds))
                    .Setup());
           
            // Assert
            var response = await HttpClient.GetAsync("test");
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        
        [Fact]
        public async Task Should_Reset_Expecation_On_MockServer()
        {
            // Arrange
            await MockClient.SetupAsync(exp =>
                exp.OnHandling(HttpMethod.Get, req => req.WithPath("/test"))
                    .RespondOnce(HttpStatusCode.Created, resp => resp.WithDelay(50, TimeUnit.Milliseconds))
                    .Setup());

            // Act
            await MockClient.ResetAsync();

            // Assert
            var response = await HttpClient.GetAsync("test");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_Verify_Expecation_Was_Met_On_MockServer()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
            await HttpClient.SendAsync(request);

            // Act
            await MockClient.VerifyAsync(v => v
                .WithMethod(HttpMethod.Get)
                .WithPath("test"), VerificationTimes.MoreThan(1));
        }

    }
}
