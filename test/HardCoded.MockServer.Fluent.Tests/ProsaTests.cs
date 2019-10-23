using System.Net;
using System.Net.Http;

using HardCoded.MockServer.Fluent.Builder.Expectation;

using Xunit;
using Xunit.Abstractions;

namespace HardCoded.MockServer.Fluent.Tests
{
    

    public sealed class ProsaTests
    {
        private readonly ITestOutputHelper _output;

        public ProsaTests(ITestOutputHelper output)
        {
            _output = output;
        }


        [Fact]
        public void Should_Setup_Multiple_Expectationst_At_Once()
        {

            var builder = new FluentExpectationBuilder();

            var setup = builder
                        .OnHandling(
                            HttpMethod.Delete, request => request
                                        .WithContent(content => content.MatchingXPath("//id"))
                                        .WithPath("post")
                                        .EnableEncryption()
                                        .KeepConnectionAlive())
                        .RespondWith(
                            HttpStatusCode.Accepted, response => response
                                        .WithBody(
                                            body => body.WithExactString("hello world!")
                                                        .WithContentType("text/plain"))
                                        .WithDelay(delay => delay.FromMinutes(1)))
                        .And
                        .OnHandling(
                            HttpMethod.Delete, request => request
                                        .WithPath("post")
                                        .EnableEncryption()
                                        .KeepConnectionAlive())
                        .RespondWith(
                            HttpStatusCode.Accepted, response => response
                                        .WithBody(body => body.MatchingXPath("//id")))
                        .Setup();

            var json = setup.ToString();
            _output.WriteLine(json);
        }
    }
}
