using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Requests;
using Xunit;

namespace NinjaTools.FluentMockServer.Tests
{
    public class ManualMockServerSetupTests
    {
        [Fact(Skip = "Does not work in build pipeline")]
        public async Task Should_Successfully_Setup_Up_Expecation_On_MockServer()
        {
            // Arrange
            using var client = new MockServerClient("http://localhost:5000");
          
            // Act
            await client.SetupAsync(exp =>
                exp.OnHandling(HttpMethod.Get, request => request.WithPath("/test"))
                    .RespondOnce(201, response => response.WithDelay(50, TimeUnit.Milliseconds))
                    .Setup());
           
            // Assert
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
            var response = await client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        
        [Fact(Skip = "Does not work in build pipeline")]
        public async Task Should_Reset_Expecation_On_MockServer()
        {
            // Arrange
            using var client = new MockServerClient("http://localhost:5000");
        
            
            await client.SetupAsync(exp =>
                exp.OnHandling(HttpMethod.Get, request => request.WithPath("/test"))
                    .RespondOnce(201, response => response.WithDelay(50, TimeUnit.Milliseconds))
                    .Setup());
            
            // Act
            var result = await client.Reset();
            
            // Assert
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
            var response = await client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        
        [Fact(Skip = "Does not work in build pipeline")]
        public async Task Should_Verify_Expecation_Was_Met_On_MockServer()
        {
            // Arrange
            using var client = new MockServerClient("http://localhost:5000");
 
            await client.SetupAsync(exp =>
                exp.OnHandling(HttpMethod.Get, request => request.WithPath("/test"))
                    .RespondOnce(201, response => response.WithDelay(50, TimeUnit.Milliseconds))
                    .Setup());

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            // Act
            var verification = VerificaionRequest.Once( new HttpRequest(){ Path = "test", HttpMethod = HttpMethod.Get});
            response = await client.Verify(verification);
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        }
        
        
    }
}
