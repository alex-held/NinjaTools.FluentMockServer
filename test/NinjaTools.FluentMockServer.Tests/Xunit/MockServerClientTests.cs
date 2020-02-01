using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NinjaTools.FluentMockServer.Serialization;
using NinjaTools.FluentMockServer.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Xunit
{
    public class MockServerClientTests : MockServerTestBase
    {
        /// <inheritdoc />
        public MockServerClientTests(MockServerFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }


        [Fact]
        public async Task ListSetupsAsync_Returns_One_Active_Setups_When_One_Setup_Is_Active()
        {
            // Arrange
            await MockClient.SetupAsync(_ => _.OnHandlingAny(HttpMethod.Patch).RespondOnce(HttpStatusCode.OK).Setup());

            // Act
            var response = await MockClient.ListSetupsAsync();

            // Assert
            response.Should().ContainSingle().Subject.HttpRequest.Method.Should().Be("PATCH");
        }


        [Fact]
        public async Task ListRequestsAsync_Returns_All_Requests_The_MockServer_Received()
        {
            // Arrange
            await HttpClient.GetAsync("a");
            await HttpClient.PostAsync("b", new JsonContent(new
            {
                Type = "Request",
                Value = 1
            }));
            await HttpClient.GetAsync("c");

            // Act
            var response = await MockClient.ListRequestsAsync();

            // Assert
            response.Should().HaveCount(3);

            var a = response.First();
            a.Path.Should().Be("/a");
            a.Method.Should().Be("GET");

            var b  = response.ElementAt(1);
            Dump(b, "B");
            b.Path.Should().Be("/b");
            b.Method.Should().Be("POST");
            b.Body.Value<string>("type").Should().Be("STRING");
            var bodyValue = b.Body.Value<string>("string");
            bodyValue.Should().Be("{\n  \"type\": \"Request\",\n  \"value\": 1\n}");

            var c  = response.Last();
            c.Path.Should().Be("/c");
            c.Method.Should().Be("GET");
        }


        [Fact]
        public async Task RemoveSetupsAsync_Removes_Active_Setups_When_Match()
        {
            // Arrange
            await MockClient.SetupAsync(_ => _.
                OnHandlingAny(HttpMethod.Get).RespondOnce(HttpStatusCode.OK)
                .And
                .OnHandlingAny(HttpMethod.Post).RespondOnce(HttpStatusCode.NoContent)
                .Setup());

            // Act
            await MockClient.RemoveSetupsAsync(_ => _.WithMethod(HttpMethod.Get));

            // Assert
            var response = await MockClient.ListSetupsAsync();
            response.Should().ContainSingle();
            response.Single().HttpRequest.Method.Should().Be("POST");
        }
    }
}
