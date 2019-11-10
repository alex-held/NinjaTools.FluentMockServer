using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NinjaTools.FluentMockServer.Builders;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Requests;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.TestContainers.Tests
{
    public class MockServerFixtureTests : MockServerFixture
    {
        /// <inheritdoc />
        public MockServerFixtureTests(MockServerRunner mockServerRunner, ITestOutputHelper outputHelper)
            : base(mockServerRunner, outputHelper)
        {
            _outputHelper = outputHelper;
        }

        private readonly ITestOutputHelper _outputHelper;


        [Fact]
        public async Task Should_Reset_Expecation_On_MockServer()
        {
            // Arrange
            await Using(async client =>
            {
                await client.SetupAsync(exp =>
                    exp.OnHandling(HttpMethod.Get, request => request.WithPath("/test"))
                        .RespondOnce(201, response => response.WithDelay(50, TimeUnit.Milliseconds))
                        .Setup());
                // Act
                await client.Reset();

                // Assert
                var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
                var response = await client.SendAsync(request);

                response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            });
        }

        [Fact]
        public async Task Should_Successfully_Setup_Up_Expectation_On_MockServer()
        {
            // Arrange
            await Using(async client =>
            {
                await client.SetupAsync(exp =>
                    exp.OnHandling(HttpMethod.Get, request => request.WithPath("/test"))
                        .RespondOnce(201, response => response.WithDelay(50, TimeUnit.Milliseconds))
                        .Setup());

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
                .RespondWith(HttpStatusCode.Created, resp => resp.WithDelay(50, TimeUnit.Milliseconds))
                .Setup();

            var builder = new FluentExpectationBuilder(new MockServerSetup());

            foreach (var setupExpectation in factory(builder).Expectations)
            {
                _outputHelper.WriteLine(setupExpectation.Serialize());
            }

            // Act
            await Using(async client =>
            {
                var expectation = factory(builder).Expectations.First();
                _outputHelper.WriteLine(expectation.Serialize());

                await client.SetupAsync(factory);

                var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/test", UriKind.Relative));
                var response = await client.SendAsync(request);
                response.StatusCode.Should().Be(HttpStatusCode.Created);
            });
        }

        [Fact]
        public async Task Should_Verify_Expectation_Was_Met_On_MockServer()
        {
            // Arrange

            await Using(async client =>
            {
                await client.SetupAsync(exp =>
                    exp.OnHandling(HttpMethod.Get, request => request.WithPath("/test"))
                        .RespondOnce(201, response => response.WithDelay(50, TimeUnit.Milliseconds))
                        .Setup());

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
