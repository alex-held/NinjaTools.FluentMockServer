using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.FluentAPI;
using NinjaTools.FluentMockServer.FluentAPI.Builders;
using NinjaTools.FluentMockServer.FluentAPI.Builders.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Serialization;
using NinjaTools.FluentMockServer.Xunit;
using NinjaTools.FluentMockServer.Xunit.Attributes;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Xunit
{
    public class MockServerFixtureTests : MockServerTestBase
    {
        /// <inheritdoc />
        public MockServerFixtureTests([NotNull] MockServerFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        [Fact]
        public async Task Should_Reset_Expectation_On_MockServer()
        {
            // Arrange
           await MockClient.SetupAsync(exp =>
                    exp.OnHandling(HttpMethod.Get, req => req.WithPath("/test"))
                        .RespondOnce(HttpStatusCode.Created, resp => resp.WithDelay(50, TimeUnit.Milliseconds))
                        .Setup());
            // Act
            await MockClient.ResetAsync();

                // Assert
                var response = await HttpClient.GetAsync("test");

                response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        }

        [Fact]
        public async Task Should_Successfully_Setup_Up_Expectation_On_MockServer()
        {
            // Arrange
            await MockClient.SetupAsync(exp =>
                exp.OnHandling(HttpMethod.Get, req => req.WithPath("/test"))
                    .RespondOnce(HttpStatusCode.Created, resp => resp.WithDelay(50, TimeUnit.Milliseconds))
                    .Setup());

            var response = await HttpClient.GetAsync("test");
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

            var response = await HttpClient.GetAsync("/test");
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        [IsolatedMockServerSetup]
        public async Task Should_Verify_Expectation_Was_Met_On_MockServer()
        {
            // Arrange
            await MockClient.SetupAsync(exp =>
                    exp.OnHandling(HttpMethod.Get, req => req.WithPath("/test"))
                        .RespondOnce(HttpStatusCode.Created, resp => resp.WithDelay(50, TimeUnit.Milliseconds))
                        .Setup());

                await HttpClient.GetAsync("test");
                var builder = new FluentHttpRequestBuilder();
                builder.WithMethod(HttpMethod.Get).WithPath("/test");

                // Act
                 await MockClient.VerifyAsync( v => v
                    .WithPath("/test")
                    .WithMethod(HttpMethod.Get), VerificationTimes.Once);
        }
    }
}
