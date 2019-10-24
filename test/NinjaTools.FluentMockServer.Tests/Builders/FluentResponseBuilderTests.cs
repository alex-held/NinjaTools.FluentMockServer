using FluentAssertions;

using NinjaTools.FluentMockServer.Builders;

using Xunit;
using Xunit.Abstractions;


namespace NinjaTools.FluentMockServer.Tests.Builders
{
    public class FluentResponseBuilderTests
    {
        private readonly ITestOutputHelper _outputHelper;
        
        public FluentResponseBuilderTests(ITestOutputHelper outputHelper) { _outputHelper = outputHelper; }
        
        [Fact]
        public void Should_Add_Header()
        {
            // Arrange
            var builder = new FluentHttpResponseBuilder();
            
            // Act
            var response = builder
                        .WithHeader("Content-Type",  "text/xml charset=UTF-8;")
                        .Build()
                        .Serialize();
            
            // Assert
            _outputHelper.WriteLine(response);
            response.Should().MatchRegex(@"(?mis){.*""Content-Type"":.*\[.*""text/xml charset=UTF-8;"".*\].*}.*");

        }
        
        [Fact]
        public void Should_Add_Body_Literal()
        {
            // Arrange
            var builder = new FluentHttpResponseBuilder();
            
            // Act
            var response = builder
                .WithBodyLiteral("Hello World!")
                .Build()
                .Serialize();
            
            // Assert
            _outputHelper.WriteLine(response);
            response.Should().MatchRegex(@"{\s*""body"":\s*""Hello World!""\s*}\s*");

        }
        
        [Fact]
        public void Should_Add_Multiple_Header_Values()
        {
            // Arrange
            var builder = new FluentHttpResponseBuilder();
            
            // Act
            var response = builder
                        .WithHeader("Content-Type", "text/xml charset=UTF-8;")
                        .WithHeader("Header-name 2", "true")
                        .Build()
                        .Serialize();
            
            // Assert
            _outputHelper.WriteLine(response);
            response.Should().MatchRegex(@"(?mis){.*""Content-Type"":.*\[.*""text/xml charset=UTF-8;"".*\].*,.*""Header-name 2"":.*\[.*""true"".*].*}.*");
        }
    }
}
