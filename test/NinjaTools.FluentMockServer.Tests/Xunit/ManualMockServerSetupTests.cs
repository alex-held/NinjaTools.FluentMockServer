using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Xunit;
using Xunit;
using Xunit.Abstractions;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly, DisableTestParallelization = true)]
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
                    .RespondOnce(201, resp => resp.WithDelay(50, TimeUnit.Milliseconds))
                    .Setup());
           
            // Assert
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
            var response = await MockClient.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        
        [Fact]
        public async Task Should_Reset_Expecation_On_MockServer()
        {
            // Arrange
            await MockClient.SetupAsync(exp =>
                exp.OnHandling(HttpMethod.Get, req => req.WithPath("/test"))
                    .RespondOnce(201, resp => resp.WithDelay(50, TimeUnit.Milliseconds))
                    .Setup());

            // Act
            await MockClient.ResetAsync();

            // Assert
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
            var response = await MockClient.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_Verify_Expecation_Was_Met_On_MockServer()
        {
            // Act
            await MockClient.SetupAsync(exp =>
                exp.OnHandling(HttpMethod.Get, req => req.WithPath("test"))
                    .RespondOnce(201, resp => resp.WithDelay(50, TimeUnit.Milliseconds))
                    .Setup());

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
            var response = await MockClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // Act
            var (isValid, responseMessage) = await MockClient.VerifyAsync(v => v
                .WithMethod(HttpMethod.Get)
                .WithPath("test"), VerificationTimes.Once);
            
            // Assert
            isValid.Should().BeTrue();
        }

    }
}
