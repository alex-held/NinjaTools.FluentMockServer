using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.API.Tests.Serilization
{
    public class HeadersTests : XUnitTestBase<HeadersTests>
    {
        public HeadersTests(ITestOutputHelper output) :  base(output)
        { }

        [Fact]
        public void Should_Serialize()
        {
            // Arrange
            const string expected =
@"{
  ""a"": [
    ""1""
  ],
  ""b"": [
    ""1""
  ],
  ""c"": [
    ""1""
  ],
  ""d"": [
    ""1""
  ]
}";
            var instance = new Headers(("a", new []{"1"}),("b", new []{"1"}),("c", new []{"1"}),("d", new []{"1"}));

            // Act
            var jo = JObject.FromObject(instance);
            Dump(jo, "Headers - JObject");
            var json = jo.ToString(Formatting.Indented);

            // Assert
            Output.WriteLine(json);
            json.Should().Be(expected);
        }


        [Fact]
        public void Should_Deserialize()
        {
            // Arrange
            var expected =  new Headers(("a", new []{"1"}),("b", new []{"1"}),("c", new []{"1"}),("d", new []{"1"}));

            // Act
            var instance = JObject.Parse(@"{
  ""a"": [
    ""1""
  ],
  ""b"": [
    ""1""
  ],
  ""c"": [
    ""1""
  ],
  ""d"": [
    ""1""
  ]
}");
            Dump(instance, "HeaderCollection - JObject");
            var dict = instance.ToObject<Headers>();

             // Assert
             dict.Should().HaveSameCount(expected.Header);
        }
    }
}
