using System.Collections.Generic;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Serialization
{
    public class ExpectationSerializationTests
    {
        private ILogger<ExpectationSerializationTests> _logger;

        public ExpectationSerializationTests(ITestOutputHelper outputHelper)
        {
            _logger = LoggerFactory
                .Create(i => i.AddXunit(outputHelper))
                .CreateLogger<ExpectationSerializationTests>();
        }


        [Fact]
        public void SerializeJObject_Should_Use_SerializeJObject_Of_Properties()
        {
            // Arrange
            var expectation = new Expectation
            {
                HttpRequest = new HttpRequest
                {
                    Body = RequestBody.MatchXml("<xml here>"),
                    Path = "",
                    Method = HttpMethod.Post.Method
                },
                HttpResponse = new HttpResponse
                {
                    Headers = new Dictionary<string, string[]>
                    {
                        {"Content-Type", new[] {"xml"}}
                    },
                    Body = new JValue("some xml response"),
                    Delay = new Delay
                    {
                        Value = 50,
                        TimeUnit = TimeUnit.Milliseconds
                    },
                    StatusCode = 200
                }
            };
            
            
            // Act
            var jo = JObject.FromObject(expectation);
            var json = expectation.ToString();

            _logger.LogInformation("JSON", json);
            
            // Assert
            jo["httpResponse"]["body"].ToObject<string>().Should().Be("some xml response");
        }
    }
}
