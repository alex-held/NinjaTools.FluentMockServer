using FluentAssertions;
using Microsoft.Extensions.Logging;
using NinjaTools.FluentMockServer.Builders;
using NinjaTools.FluentMockServer.Builders.Request;
using NinjaTools.FluentMockServer.Extensions;
using NinjaTools.FluentMockServer.Tests.Test;
using Xunit;
using Xunit.Abstractions;


namespace NinjaTools.FluentMockServer.Tests.Builders
{
    public class FluentRequestBuilderTests
    {
        private readonly ILogger _logger;


        public FluentRequestBuilderTests(ITestOutputHelper logger)
        {
            _logger = LoggerFactory.Create(l => l.AddXunit(logger)).CreateLogger(nameof(FluentRequestBuilderTests));
        }


        [Fact]
        public void Should_Use_Encryption()
        {
            // Arrange
            var builder = new FluentHttpRequestBuilder();
            
            // Act
            var result = builder.EnableEncryption().Build().AsJson();
            _logger.LogResult("JSON", result);
            
            // Assert
            result.Should().MatchRegex(@"(?ms){.*""secure"": true.*}.*");
        }
        
        
        [Fact]
        public void  Should_Set_Content_Type_Header()
        {
            // Arrange
            var builder = new FluentHttpRequestBuilder();
            
            // Act
            var result = builder
                .AddContentType(CommonContentType.Soap12)
                .Build();

            _logger.LogResult("JSON ", result.AsJson());
            
            // Assert
            // TODO:reactive
        }

        
        [Fact]
        public void Should_Keep_Alive()
        {
            // Arrange
            var builder = new FluentHttpRequestBuilder();
            
            // Act
            var result = builder.KeepConnectionAlive().Build().AsJson();
            _logger.LogResult("JSON", result);
            
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
            var result = builder.WithPath(inputPath).Build().AsJson();
            _logger.LogResult("JSON", result);
            
            // Assert
            result.Should().MatchRegex($@"(?ms){{.*""path"":.*""{expectedPath}"".*}}.*");
        }

    }
}
