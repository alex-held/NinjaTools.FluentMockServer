using JetBrains.Annotations;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using NinjaTools.FluentMockServer.Tests.TestHelpers.Data.TheoryData;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Serialization
{
    public class HttpRequestSerializationTests : XUnitTestBase
    {
        public HttpRequestSerializationTests(ITestOutputHelper output) : base(output)
        { }
        
        [Theory]
        [ClassData(typeof(RandomHttpRequestData))]
        public void Should_Serialize_HttpRequests_As_Expected([NotNull] HttpRequest request)
        {
            // Act & Assert
            var json = JsonConvert.SerializeObject(request, Formatting.Indented);
            Output.WriteLine(json);
        }
    }
}
