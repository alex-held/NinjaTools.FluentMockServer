using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NinjaTools.FluentMockServer.FluentAPI.Builders.HttpEntities;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly, DisableTestParallelization = true)]
namespace NinjaTools.FluentMockServer.TestContainers.Tests
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
                exp.OnHandling(HttpMethod.Get, req => req.WithPath("/test"))
                    .RespondOnce(201, resp => resp.WithDelay(50, TimeUnit.Milliseconds))
                    .Setup());

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
            var response = await MockClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // Act
            var builder = new FluentHttpRequestBuilder();
            builder.WithMethod(HttpMethod.Get).WithPath("test");

            // Act
            var verification = Verify.Once(builder.Build());
            response = await MockClient.VerifyAsync(verification);
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        }

    }
}
