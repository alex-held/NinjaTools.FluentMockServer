using System;
using System.Net;
using System.Net.Http;

using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace HardCoded.MockServer.Fluent.Tests
{
    

    public class UnitTest1
    {
        private readonly ITestOutputHelper _output;

        public UnitTest1(ITestOutputHelper output)
        {
            _output = output;
        }


        [Fact]
        public void Test3()
        {

            var expectation = MockServerSetup.Expectations
                                             .OnHandling(HttpMethod.Delete,
                                                         request =>
                                                             request
                                                                .WithPath("post")
                                                                .EnableEncryption()
                                                                .KeepConnectionAlive()
                                              )
                                             .RespondWith(HttpStatusCode.Accepted,
                                                          response => response
                                                             .WithBody(content => content.WithJson(""))
                                              )
                                             .And
                                             .OnHandling(HttpMethod.Delete,
                                                         request =>
                                                             request
                                                                .WithPath("post")
                                                                .EnableEncryption()
                                                                .KeepConnectionAlive()
                                              )
                                             .RespondWith(HttpStatusCode.Accepted,
                                                          response => response
                                                             .WithBody(content => content.WithJson(""))
                                              ).Setup();

        var json = JsonConvert.SerializeObject(expectation, Formatting.Indented);
            _output.WriteLine(json);
        }
    }
}