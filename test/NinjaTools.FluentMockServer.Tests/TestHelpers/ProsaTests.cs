using System.Net;
using System.Net.Http;
using System.Text;
using NinjaTools.FluentMockServer.FluentAPI.Builders;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.TestHelpers
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
                        .OnHandling(HttpMethod.Delete, request => request
                                        .WithContent(content => content.MatchingXPath("//id"))
                                        .WithPath("post")
                                        .EnableEncryption()
                                        .KeepConnectionAlive())
                        .RespondWith(HttpStatusCode.Accepted, response => response
                            .ConfigureConnection( opt => opt.Build())
                            .ConfigureHeaders(opt =>
                            {
                                opt.AddContentType(CommonContentType.Json);
                                opt.Add("Basic", "cqwr");
                            })
                            .WithStatusCode(100)
                            .WithBody("hello world!")
                            .WithDelay(1, TimeUnit.Minutes))
                        .And
                        .OnHandling(
                            HttpMethod.Delete, request => request
                                        .WithPath("post")
                                        .EnableEncryption()
                                        .KeepConnectionAlive())
                        .RespondWith(HttpStatusCode.Accepted, response => response
                            .WithBody(Encoding.UTF8.GetBytes("ewogICAgIk5hbWUiOiAiQWxleCIKfQ==")))
                        .Setup();

            var json = setup.ToString();
            _output.WriteLine(json);
        }
    }
}
