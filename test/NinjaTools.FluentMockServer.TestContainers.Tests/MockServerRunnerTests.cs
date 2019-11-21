using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NinjaTools.FluentMockServer.Builders;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Requests;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.TestContainers.Tests
{
    public class MockServerFixtureTests : IClassFixture<MockServerFixture>
    {
        /// <inheritdoc />
        public MockServerFixtureTests(MockServerFixture mockServerFixture, ITestOutputHelper outputHelper)
        {
            _mockServerFixture = mockServerFixture;
            _outputHelper = outputHelper;
        }

        private readonly MockServerFixture _mockServerFixture;
        private readonly ITestOutputHelper _outputHelper;

        protected MockServerClient Client => _mockServerFixture.Client;
        
        
        [Fact]
        public async Task Should_Reset_Expectation_On_MockServer()
        {
            
            // Arrange
           await Client.SetupAsync(exp =>
                    exp.OnHandling(HttpMethod.Get, request => request.WithPath("/test"))
                        .RespondOnce(201, response => response.WithDelay(50, TimeUnit.Milliseconds))
                        .Setup());
            // Act
            await Client.Reset();

                // Assert
                var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
                var response = await Client.SendAsync(request);

                response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            
        }

        [Fact]
        public async Task Should_Successfully_Setup_Up_Expectation_On_MockServer()
        {
            // Arrange
          
                await Client.SetupAsync(exp =>
                    exp.OnHandling(HttpMethod.Get, request => request.WithPath("/test"))
                        .RespondOnce(201, response => response.WithDelay(50, TimeUnit.Milliseconds))
                        .Setup());

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
            var response = await Client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }


        [Fact]
        public async Task Should_Successfully_Setup_Up_Expectation_On_MockServer2()
        {
            // Arrange
            Func<IFluentExpectationBuilder, MockServerSetup> factory = e => e
                .OnHandling(HttpMethod.Get, req => req.WithPath("/test"))
                .RespondWith(HttpStatusCode.Created, resp => resp.WithDelay(50, TimeUnit.Milliseconds))
                .Setup();

            var builder = new FluentExpectationBuilder(new MockServerSetup());

            foreach (var setupExpectation in factory(builder).Expectations)
            {
                _outputHelper.WriteLine(setupExpectation.Serialize());
            }

            // Act
          
            var expectation = factory(builder).Expectations.First();
            _outputHelper.WriteLine(expectation.Serialize());

            await Client.SetupAsync(factory);

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/test", UriKind.Relative));
            var response = await Client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Should_Verify_Expectation_Was_Met_On_MockServer()
        {
            // Arrange
            await Client.SetupAsync(exp =>
                    exp.OnHandling(HttpMethod.Get, request => request.WithPath("/test"))
                        .RespondOnce(201, response => response.WithDelay(50, TimeUnit.Milliseconds))
                        .Setup());

                var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
                await Client.SendAsync(request);

                // Act
                var verification = VerificaionRequest.Once(new HttpRequest
                {
                    HttpMethod = HttpMethod.Get,
                    Path = "/test"
                });
                var response = await Client.Verify(verification);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        }
    }
}
