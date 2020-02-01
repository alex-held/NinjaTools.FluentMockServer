using System.Collections.Generic;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.FluentAPI.Builders;
using NinjaTools.FluentMockServer.FluentAPI.Builders.HttpEntities;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Serialization;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Serialization
{
    public class ExpectationSerializationTests : XUnitTestBase<ExpectationSerializationTests>
    {

        public ExpectationSerializationTests(ITestOutputHelper output) : base(output)
        { }


        [Fact]
        public void SerializeJObject_Should_Use_SerializeJObject_Of_Properties()
        {
            // Arrange
            var builder = new FluentHttpRequestBuilder()
                .WithPath("")
                .WithMethod(HttpMethod.Post)
                .WithContent(body => body.WithoutXmlContent("<xml here>"));

            var expectation = FluentExpectationBuilder.Create(builder.Build(), new HttpResponse
                {
                    Headers = new Dictionary<string, string[]>
                    {
                        {"Content-Type", new[] {"xml"}}
                    },
                    Body = new JValue("some xml response"),
                    Delay = new Delay(TimeUnit.Milliseconds, 50),
                    StatusCode = 200
                }
            );
            
            
            // Act
            var jo = Serializer.SerializeJObject(expectation);
            var json = Serializer.Serialize(expectation);

            Logger.LogInformation("JSON", json);
            
            // Assert
            jo["httpResponse"]["body"].ToObject<string>().Should().Be("some xml response");
        }
    }
}
