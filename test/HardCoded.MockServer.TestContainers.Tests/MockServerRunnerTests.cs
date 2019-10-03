using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using HardCoded.MockServer.Models.HttpEntities;
using Xunit;
using Xunit.Abstractions;

namespace HardCoded.MockServer.TestContainers.Tests
{
   
    public class MockServerFixtureTests : MockServerFixture
    {
        [Fact]
        public async Task Should_Log_Information_Into_Console()
        {
            Logger.LogInformation("Hello World!");
        }

        /// <inheritdoc />
        public MockServerFixtureTests(MockServerRunner mockServerRunner, ITestOutputHelper outputHelper) 
            : base(mockServerRunner, outputHelper)
        {
        }
        
        

        [Fact]
        public async Task Should_Successfully_Setup_Up_Expectation_On_MockServer()
        {
            // Arrange
            await Using(async (client) =>
            {
                await client.SetupExpectationAsync(new Expectation
                {
                    HttpRequest = HttpRequest.Get("/test"),
                    HttpResponse = new HttpResponse(201)
                });

                var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
                var response = await client.SendAsync(request);
                response.StatusCode.Should().Be(HttpStatusCode.Created);
            });
        }
        
        [Fact]
        public async Task Should_Reset_Expecation_On_MockServer()
        {
            // Arrange
            await Using(async (client) =>
            {
                await client.SetupExpectationAsync(new Expectation
                {
                    HttpRequest = HttpRequest.Get("/test"),
                    HttpResponse = new HttpResponse(201)
                });

                // Act
                await client.Reset();

                // Assert
                var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
                var response = await client.SendAsync(request);

                response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            });
        }
        
        [Fact]
        public async Task Should_Verify_Expectation_Was_Met_On_MockServer()
        {
            // Arrange

            await Using(async (client) =>
            {
                await client.SetupExpectationAsync(new Expectation
                {
                    HttpRequest = HttpRequest.Get("/test"),
                    HttpResponse = new HttpResponse(201)
                });

                var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
                await client.SendAsync(request);

                // Act
                var verification = VerificaionRequest.Once(HttpRequest.Get("/test"));
                var response = await client.Verify(verification);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Accepted);
            });
        }
    }
}