using FluentAssertions;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Extensions;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Serialization
{
    public class HttpResponseSerializationTests : XUnitTestBase<HttpResponseSerializationTests>
    {
        /// <inheritdoc />
        public HttpResponseSerializationTests(ITestOutputHelper output) : base(output)
        {
        }
        
        [Fact]
        public void Get_Body_Returns_Null_If_Content_Is_Null()
        {
            // Arrange
            var response = HttpResponse.Create();
            
            // Act & Assert<
            response.Body.Should().BeNull($"{nameof(HttpResponse.Body)} is null.");
        }
        
        
        [Fact]
        public void Get_Body_Returns_JProperty_When_Set_To_Literal()
        {
            // Arrange
            var expected = new JObject(new JProperty("body", "Hello World!"));
            
            // Act
            var response = HttpResponse.Create(body: new JValue("Hello World!"));

            // Assert
            var jo = response.AsJObject();
            Output.Dump(jo);
            jo.Should().BeEquivalentTo(expected);
        }

     
    }
}
