using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NinjaTools.FluentMockServer.FluentAPI;
using NinjaTools.FluentMockServer.FluentAPI.Builders;
using NinjaTools.FluentMockServer.FluentAPI.Builders.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Serialization;
using NinjaTools.FluentMockServer.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Xunit
{
    public class MockServerFixtureTests  : MockServerTestBase
    {
        /// <inheritdoc />
        public MockServerFixtureTests(MockServerFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        [Fact]
        public async Task Should_Reset_Expectation_On_MockServer()
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
        public async Task Should_Successfully_Setup_Up_Expectation_On_MockServer()
        {
            // Arrange
          
                await MockClient.SetupAsync(exp =>
                    exp.OnHandling(HttpMethod.Get, req => req.WithPath("/test"))
                        .RespondOnce(201, resp => resp.WithDelay(50, TimeUnit.Milliseconds))
                        .Setup());

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
            var response = await MockClient.SendAsync(request);
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
                Output.WriteLine(Serializer.Serialize(setupExpectation));
            }

            // Act
          
            var expectation = factory(builder).Expectations.First();
            Output.WriteLine(Serializer.Serialize(expectation));

            await MockClient.SetupAsync(factory);

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("/test", UriKind.Relative));
            var response = await MockClient.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Should_Verify_Expectation_Was_Met_On_MockServer()
        {
            // Arrange
            await MockClient.SetupAsync(exp =>
                    exp.OnHandling(HttpMethod.Get, req => req.WithPath("/test"))
                        .RespondOnce(201, resp => resp.WithDelay(50, TimeUnit.Milliseconds))
                        .Setup());

                var request = new HttpRequestMessage(HttpMethod.Get, new Uri("test", UriKind.Relative));
                await MockClient.SendAsync(request);
                var builder = new FluentHttpRequestBuilder();
                builder.WithMethod(HttpMethod.Get).WithPath("/test");
                
                // Act
                var (isValid, response) = await MockClient.VerifyAsync( v => v
                     .WithPath("/test").WithMethod(HttpMethod.Get), VerificationTimes.Once);

                // Assert
                isValid.Should().BeTrue();
        }
    }
}
