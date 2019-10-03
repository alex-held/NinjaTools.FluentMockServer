using System.Collections.Generic;
using FluentAssertions;
using HardCoded.MockServer.HttpBodies;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace TestServer.Fluent
{
    public class FluentApiTests
    {
        private readonly ITestOutputHelper _logger;

        public FluentApiTests(ITestOutputHelper logger)
        {
            _logger = logger;
        }
        
        [Theory]
        [InlineData(MatchType.STRICT)]
        [InlineData(MatchType.ONLY_MATCHING_FIELDS)]
        [InlineData(null)]
        public void Should_Serialize_Json(MatchType? matchType)
        {
            var request = new RequestContext()
                .WithJson("{\"Hallo\": \"Test\"}", matchType);

            var serialized = JsonConvert.SerializeObject(request.Body, Formatting.Indented);
           
            var dict = new Dictionary<string, object>();
            dict.Add("type", "JSON");
            dict.Add("json", "{\"Hallo\": \"Test\"}");
            if (matchType.HasValue)
                dict.Add("matchType", matchType.Value.ToString().ToUpper());
         
            var expected = JsonConvert.SerializeObject(dict, Formatting.Indented);

            _logger.WriteLine(expected);
            serialized.Should().Be(expected);
        }
        
        
        [Fact]
        public void Should_Serialize_Xml()
        {
            var request = new RequestContext()
                .WithXml("<bookstore> <book nationality=\"ITALIAN\" category=\"COOKING\"><title lang=\"en\">Everyday Italian</title><author>Giada De Laurentiis</author><year>2005</year><price>30.00</price></book> </bookstore>");

            var serialized = JsonConvert.SerializeObject(request.Body);
           
            var dict = new Dictionary<string, object>();
            dict.Add("type", "XML");
            dict.Add("xml", "<bookstore> <book nationality=\"ITALIAN\" category=\"COOKING\"><title lang=\"en\">Everyday Italian</title><author>Giada De Laurentiis</author><year>2005</year><price>30.00</price></book> </bookstore>");
            var expected = JsonConvert.SerializeObject(dict);

            _logger.WriteLine(expected);
            serialized.Should().Be(expected);
        }
    }
}