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
    
    public class ManualMockServerSetupTests : XUnitTestBase<ManualMockServerSetupTests>, IClassFixture<MockServerFixture>
    {
        /// <inheritdoc />
        public ManualMockServerSetupTests(ITestOutputHelper output, MockServerFixture fixture) : base(output)
        {
            _fixture = fixture;
        }
        
        private readonly MockServerFixture _fixture;
        private MockServerClient Client => _fixture.Client;

        [Fact]
        public async Task Should_Successfully_Setup_Up_Expecation_On_MockServer()
        {
            // Act
            await Client.SetupAsync(exp =>
                exp.OnHandling(HttpMethod.Get, req => req.WithPath("/test"))
                    .RespondOnce(201, resp => resp.WithDelay(50, TimeUnit.Milliseconds))
                    .Setup());
           
            // Assert
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
            var response = await Client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        
        [Fact]
        public async Task Should_Reset_Expecation_On_MockServer()
        {
            // Arrange
            await Client.SetupAsync(exp =>
                exp.OnHandling(HttpMethod.Get, req => req.WithPath("/test"))
                    .RespondOnce(201, resp => resp.WithDelay(50, TimeUnit.Milliseconds))
                    .Setup());
            
            // Act
            await Client.ResetAsync();
            
            // Assert
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
            var response = await Client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        
        [Fact]
        public async Task Should_Verify_Expecation_Was_Met_On_MockServer()
        {
            // Act
            await Client.SetupAsync(exp =>
                exp.OnHandling(HttpMethod.Get, req => req.WithPath("/test"))
                    .RespondOnce(201, resp => resp.WithDelay(50, TimeUnit.Milliseconds))
                    .Setup());

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            // Act
            var builder = new FluentHttpRequestBuilder();
            builder.WithMethod(HttpMethod.Get).WithPath("test");
                
            // Act
            var verification = Verify.Once(builder.Build());
            response = await Client.VerifyAsync(verification);
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        }

    }
}
