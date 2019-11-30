using FluentAssertions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Client.Models.HttpEntities;
using NinjaTools.FluentMockServer.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Serialization
{
    public class HttpResponseSerializationTests
    {
        private ILogger<HttpResponseSerializationTests> _logger;

        public HttpResponseSerializationTests(ITestOutputHelper outputHelper)
        {
           _logger = LoggerFactory.Create(l => l.AddXunit(outputHelper)).CreateLogger<HttpResponseSerializationTests>();
        }
        
        
        [Fact]
        public void Get_Body_Returns_Null_If_Content_Is_Null()
        {
            // Arrange
            var response = new HttpResponse();
            
            // Act & Assert<
            response.Body.Should().BeNull($"{nameof(HttpResponse.Body)} is null.");
        }
        
        
        [Fact]
        public void Get_Body_Returns_JProperty_When_Set_To_Literal()
        {
            // Arrange
            var response = new HttpResponse
            {
                Body = new JValue("Hello World!")
            };

            var expected = new JObject(new JProperty("body", "Hello World!"));
            var jo = response.AsJObject();
            jo.Should().BeEquivalentTo(expected);
        }
        
    }
}
