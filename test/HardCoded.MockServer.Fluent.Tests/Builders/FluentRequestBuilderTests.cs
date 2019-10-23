using System;
using System.Linq;
using System.Net;
using System.Net.Http;

using FluentAssertions;

using HardCoded.MockServer.Contracts.Models.HttpEntities;
using HardCoded.MockServer.Contracts.Models.ValueTypes;
using HardCoded.MockServer.Fluent.Builder.Expectation;
using HardCoded.MockServer.Fluent.Builder.Request;

using Xunit;
using Xunit.Abstractions;


namespace HardCoded.MockServer.Fluent.Tests.Builders
{
    public class FluentRequestBuilderTests
    {
        private ITestOutputHelper _outputHelper;


        public FluentRequestBuilderTests(ITestOutputHelper outputHelper) { _outputHelper = outputHelper; }


        [Fact]
        public void Should_Use_Encryption()
        {
            // Arrange
            var builder = new FluentHttpRequestBuilder();
            
            // Act
            var result = builder.EnableEncryption().Build().Serialize();
            _outputHelper.WriteLine(result);
            
            // Assert
            result.Should().MatchRegex(@"(?ms){.*""secure"": true.*}.*");
        }
        
        [Fact]
        public void Should_Keep_Alive()
        {
            // Arrange
            var builder = new FluentHttpRequestBuilder();
            
            // Act
            var result = builder.KeepConnectionAlive().Build().Serialize();
            _outputHelper.WriteLine(result);
            
            // Assert
            result.Should().MatchRegex(@"(?ms){.*""keepAlive"": true.*}.*");
        }
        
        [Theory]
        [InlineData("some/path", "/some/path")]
        [InlineData("/some/path", "/some/path")]
        [InlineData(null, "/")]
        public void Should_Set_Path(string inputPath, string expectedPath)
        {
            // Arrange
            var builder = new FluentHttpRequestBuilder();
            
            // Act
            var result = builder.WithPath(inputPath).Build().Serialize();
            _outputHelper.WriteLine(result);
            
            // Assert
            result.Should().MatchRegex($@"(?ms){{.*""path"":.*""{expectedPath}"".*}}.*");
        }

    }
}
