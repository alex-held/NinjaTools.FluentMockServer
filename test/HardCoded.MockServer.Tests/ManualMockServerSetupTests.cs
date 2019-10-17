using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using HardCoded.MockServer.Models;
using HardCoded.MockServer.Models.HttpEntities;
using HardCoded.MockServer.Requests;
using Xunit;

namespace HardCoded.MockServer.Tests
{
    public class ManualMockServerSetupTests
    {
        [Fact(Skip = "Does not work in build pipeline")]
        public async Task Should_Successfully_Setup_Up_Expecation_On_MockServer()
        {
            // Arrange
            using var client = new MockServerClient("http://localhost:5000");
            var expectation = new ExpectationRequest();
            
            expectation.Add(new Expectation
            {
                HttpRequest = HttpRequest.Get("/test"),
                HttpResponse = new HttpResponse(201)
            });

            // Act
            var response = await client.SetupExpectationAsync(expectation);
            response.EnsureSuccessStatusCode();

            // Assert
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
            response = await client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        
        [Fact(Skip = "Does not work in build pipeline")]
        public async Task Should_Reset_Expecation_On_MockServer()
        {
            // Arrange
            using var client = new MockServerClient("http://localhost:5000");
            var expectation = new ExpectationRequest();
            
            expectation.Add(new Expectation
            {
                HttpRequest = HttpRequest.Get("/test"),
                HttpResponse = new HttpResponse(201)
            });
            
            var response = await client.SetupExpectationAsync(expectation);
            response.EnsureSuccessStatusCode();
            
            // Act
            var result = await client.Reset();
            
            // Assert
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
            response = await client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        
        [Fact(Skip = "Does not work in build pipeline")]
        public async Task Should_Verify_Expecation_Was_Met_On_MockServer()
        {
            // Arrange
            using var client = new MockServerClient("http://localhost:5000");
 
            var response = await client.SetupExpectationAsync(new Expectation
            {
                HttpRequest = HttpRequest.Get("/test"),
                HttpResponse = new HttpResponse(201)
            });
            
            response.EnsureSuccessStatusCode();
            
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
            response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            // Act
            var verification = VerificaionRequest.Once(HttpRequest.Get("/test"));
            response = await client.Verify(verification);
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        }
        
        
    }
}