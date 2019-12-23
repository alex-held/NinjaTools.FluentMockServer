using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Extensions;
using NinjaTools.FluentMockServer.FluentAPI.Builders;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.FluentAPI.Builders
{
    
  
    public class FluentVerificationBuilderTests
    {
        private readonly ITestOutputHelper _outputHelper;


        public FluentVerificationBuilderTests(ITestOutputHelper outputHelper) { _outputHelper = outputHelper; }


        [Fact]
        public void Should_Verify_HttpRequest()
        {
            // Arrange
            var expected = new JObject
            {
                ["httpRequest"] = new JObject
                {
                    ["path"] = "/some/path"
                }
            }.AsJson();
            
            var builder = new FluentVerificationBuilder();
            
            // Act
            builder.Verify(request => request.WithPath("some/path"));
            var result = builder.Build().AsJson();
            
            // Assert
            _outputHelper.WriteLine(result);
            result.Should().Be(expected);
        }
        
        [Fact]
        public void Should_Verify_Once()
        {
            // Arrange
            var expected = new JObject
            {
                ["httpRequest"] = new JObject
                {
                    ["path"] = "/some/path"
                },
                ["times"] = new JObject
                {
                    ["atLeast"] = 1,
                    ["atMost"] = 1
                }
            }.ToString(Formatting.Indented);

            
            var builder = new FluentVerificationBuilder();
            
            // Act
            builder.Verify(request => request.WithPath("some/path")).Once();
            var result = builder.Build().AsJson();
            
            // Assert
            _outputHelper.WriteLine(result);
            result.Should().Be(expected);
        }
        
        [Fact]
        public void Should_Verify_Twice()
        {
            // Arrange
            var expected = new JObject
            {
                ["httpRequest"] = new JObject
                {
                    ["path"] = "/some/path"
                },
                ["times"] = new JObject
                {
                    ["atLeast"] = 2,
                    ["atMost"] = 2
                }
            }.ToString(Formatting.Indented);

            
            var builder = new FluentVerificationBuilder();
            
            // Act
            builder.Verify(request => request.WithPath("some/path")).Twice();
            var result = builder.Build().AsJson();
            
            // Assert
            _outputHelper.WriteLine(result);
            result.Should().Be(expected);
        }
        
        
        [Fact]
        public void Should_Verify_Between()
        {
            var ex = new JObject(
                new JProperty("httpRequest",new JObject(new JProperty("path", "/some/path"))),
                new JProperty(
                    "times",
                    new JObject(
                        new JProperty("atLeast", 1),
                        new JProperty("atMost", 2))));
            
            var expected = ex.ToString(Formatting.Indented);
            _outputHelper.WriteLine(expected);
            var builder = new FluentVerificationBuilder();
            
            // Act
            builder.Verify(request => request.WithPath("some/path")).Between(1, 2);
            var result = builder.Build().AsJson();
            
            // Assert
            _outputHelper.WriteLine(result);
            result.Should().Be(expected);
        }
        
        [Fact]
        public void Should_Verify_AtMost()
        {
            var expected = new JObject
            {
                ["httpRequest"] = new JObject
                {
                    ["path"] = "/some/path"
                },
                ["times"] = new JObject 
                { 
                    ["atLeast"] = 0, 
                    ["atMost"] = 5
                }
            }.ToString(Formatting.Indented);
 
            var builder = new FluentVerificationBuilder();
            
            // Act
            builder.Verify(request => request.WithPath("some/path")).AtMost(5);
            var result = builder.Build().AsJson();
            
            // Assert
            _outputHelper.WriteLine(result);
            result.Should().Be(expected);
        }
        
        [Fact]
        public void Should_Verify_AtLeast()
        {
            var expected = new JObject
            {
                ["httpRequest"] = new JObject
                {
                    ["path"] = "/some/path"
                },
                ["times"] = new JObject
                {
                    ["atLeast"] = 5
                }
            }.AsJson();
            var builder = new FluentVerificationBuilder();
            
            // Act
            builder.Verify(request => request.WithPath("some/path")).AtLeast(5);
            var result = builder.Build().AsJson();
            
            // Assert
            _outputHelper.WriteLine(result);
            result.Should().Be(expected);
        }
    }
}
