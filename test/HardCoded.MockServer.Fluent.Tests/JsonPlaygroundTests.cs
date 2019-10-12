using System.Linq;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace HardCoded.MockServer.Fluent.Tests
{
    public class JsonPlaygroundTests
    {
        private readonly ITestOutputHelper _outputHelper;
        public JsonPlaygroundTests(ITestOutputHelper outputHelper) =>  _outputHelper = outputHelper;
        
        [Fact]
        public void Should_Add_Not_To_Existing_Json()
        {
            // Arrange
            const string initial = @"{
    ""body"": {
        ""type"": ""JSON"",
        ""json"": ""{ \""id\"": 1, \""name\"": \""A green door\"", \""price\"": 12.50, \""tags\"": [\""home\"", \""green\""] }"",
        ""matchType"": ""STRICT""
    }
}";
            const string expected = @"{
    ""body"": {
        ""not"": true,
        ""type"": ""JSON"",
        ""json"": ""{ \""id\"": 1, \""name\"": \""A green door\"", \""price\"": 12.50, \""tags\"": [\""home\"", \""green\""] }"",
        ""matchType"": ""STRICT""
    }
}";
            
            // Act
            var root = JObject.Parse(initial);
            var bodyObject = root
                            .Children()
                            .OfType<JProperty>()
                            .First(prop => prop.Name == "body")
                            .Value
                            .Value<JObject>();
            
            bodyObject.AddFirst(new JProperty("not", true));
            
            _outputHelper.WriteLine("INITIAL: \n" + initial + "\n-----\n");
            _outputHelper.WriteLine("Modified: \n" + root + "\n-----\n");

            // Assert
            root.Should().BeEquivalentTo(JObject.Parse(expected));
        }

    }
}