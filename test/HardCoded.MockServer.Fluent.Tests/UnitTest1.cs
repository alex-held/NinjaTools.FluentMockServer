using System;
using System.Net.Http;
using HardCoded.MockServer.Fluent.Builder;
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
        public void Test1()
        {
            var request = HttpRequest
                         .Configure()
                         .WithMethod(HttpMethod.Get)
                         .WithPath("/test")
                         .WithCookie("cookie", "1234")
                         .EnableEncryption()
                         .KeepConnectionAlive()
                         .WithHeader("x-tracid", "1234")
                         .WithJsonContent("myjson")
                         .Build();


            var json = JsonConvert.SerializeObject(request);
            _output.WriteLine(json);
        }
    }
}