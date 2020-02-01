using System;
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
    public class MockServerClientTests : MockServerTestBase, IDisposable
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
    }
}
