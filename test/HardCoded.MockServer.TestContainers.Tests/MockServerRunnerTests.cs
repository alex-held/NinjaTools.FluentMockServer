using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

using HardCoded.MockServer.Contracts.Models;
using HardCoded.MockServer.Contracts.Models.HttpEntities;
using HardCoded.MockServer.Contracts.Models.ValueTypes;
using HardCoded.MockServer.Fluent;
using HardCoded.MockServer.Fluent.Builder.Expectation;
using HardCoded.MockServer.Requests;
using Xunit;
using Xunit.Abstractions;

namespace HardCoded.MockServer.TestContainers.Tests
{
   
    public class MockServerFixtureTests : MockServerFixture
    {
        private readonly ITestOutputHelper _outputHelper;


        /// <inheritdoc />
        public MockServerFixtureTests(MockServerRunner mockServerRunner, ITestOutputHelper outputHelper) 
            : base(mockServerRunner, outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        [Fact]
        public async Task Should_Successfully_Setup_Up_Expectation_On_MockServer()
        {
            // Arrange
            await Using(async (client) => {
                var expectation = new Expectation
                {
                            HttpRequest = new HttpRequest
                            {
                                        HttpMethod = HttpMethod.Get
                                      , Path = "/test"
                            }
                          , HttpResponse = new HttpResponse(201)
                };
                
                _outputHelper.WriteLine(expectation.Serialize());
                
                await client.SetupExpectationAsync(expectation);

                var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
                var response = await client.SendAsync(request);
                response.StatusCode.Should().Be(HttpStatusCode.Created);
            });
        }
        
            
        [Fact]
        public async Task Should_Successfully_Setup_Up_Expectation_On_MockServer2()
        {
            
            // Arrange
            Func<IFluentExpectationBuilder, MockServerSetup> factory = e => e
                        .OnHandling(HttpMethod.Get, req => req.WithPath("/test"))
                        .RespondWith(HttpStatusCode.Created, resp => resp.WithDelay(50, TimeUnit.MILLISECONDS))
                        .Setup();
            
            var builder = new FluentExpectationBuilder(new MockServerSetup());

            foreach ( var setupExpectation in factory(builder).Expectations)
                _outputHelper.WriteLine(setupExpectation.Serialize());

            // Act
            await Using(async (client) => 
            {
                var expectation = factory(builder).Expectations.First();
                _outputHelper.WriteLine(expectation.Serialize());

                await client.SetupAsync(factory);
                
                
          //      await client.SetupExpectationAsync(expectation);

                var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/test", UriKind.Relative));
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
                    HttpRequest = new HttpRequest
                    {
                        HttpMethod = HttpMethod.Get,
                        Path = "/test"
                    },
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
                    HttpRequest = new HttpRequest
                    {
                        HttpMethod = HttpMethod.Get,
                        Path = "/test"
                    },
                    HttpResponse = new HttpResponse(201)
                });

                var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
                await client.SendAsync(request);

                // Act
                var verification = VerificaionRequest.Once(new HttpRequest
                {
                    HttpMethod = HttpMethod.Get,
                    Path = "/test"
                });
                var response = await client.Verify(verification);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Accepted);
            });
        }
    }
}
