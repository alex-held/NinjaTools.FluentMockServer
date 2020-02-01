using FluentAssertions;
using NinjaTools.FluentMockServer.FluentAPI.Builders.HttpEntities;
using NinjaTools.FluentMockServer.Serialization;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.FluentAPI.Builders.HttpEntities
{
    public class FluentRequestBuilderTests : XUnitTestBase<FluentRequestBuilderTests>
    {
        public FluentRequestBuilderTests(ITestOutputHelper output) : base(output)
        {
        }


        [Fact]
        public void Should_Use_Encryption()
        {
            // Arrange
            var builder = new FluentHttpRequestBuilder();
            
            // Act
            var result = Serializer.Serialize(builder.EnableEncryption().Build());
            Logger.LogResult("JSON", result);
            
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

            Logger.LogResult("JSON ", Serializer.Serialize(result));
            
            // Assert
            // TODO:reactive
        }

        
        [Fact]
        public void Should_Keep_Alive()
        {
            // Arrange
            var builder = new FluentHttpRequestBuilder();
            
            // Act
            var result = Serializer.Serialize(builder.KeepConnectionAlive().Build());
            Logger.LogResult("JSON", result);
            
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
            var result = Serializer.Serialize(builder.WithPath(inputPath).Build());
            Logger.LogResult("JSON", result);
            
            // Assert
            result.Should().MatchRegex($@"(?ms){{.*""path"":.*""{expectedPath}"".*}}.*");
        }

    }
}
