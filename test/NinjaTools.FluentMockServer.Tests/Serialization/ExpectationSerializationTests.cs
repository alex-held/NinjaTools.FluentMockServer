using System.Collections.Generic;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.Logging;
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
                    HttpMethod = HttpMethod.Post
                },
                HttpResponse = new HttpResponse
                {
                    Headers = new Dictionary<string, string[]>
                    {
                        {"Content-Type", new[] {"xml"}}
                    },
                    Body = new LiteralContent("some xml response"),
                    Delay = new Delay
                    {
                        Value = 50,
                        TimeUnit = TimeUnit.Milliseconds
                    },
                    StatusCode = 200
                }
            };
            
            
            // Act
            var jo = expectation.SerializeJObject();
            var json = expectation.ToString();

            _logger.LogInformation("JSON", json);
            
            // Assert
            jo["httpResponse"]["body"].ToObject<string>().Should().Be("some xml response");
        }
    }
}
