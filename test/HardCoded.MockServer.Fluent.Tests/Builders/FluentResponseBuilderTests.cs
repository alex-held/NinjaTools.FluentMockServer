using FluentAssertions;

using HardCoded.MockServer.Fluent.Builder.Response;

using Xunit;
using Xunit.Abstractions;


namespace HardCoded.MockServer.Fluent.Tests.Builders
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
