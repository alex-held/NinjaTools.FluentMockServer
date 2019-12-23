using FluentAssertions;
using NinjaTools.FluentMockServer.FluentAPI;
using NinjaTools.FluentMockServer.FluentAPI.Builders.ValueObjects;
using Xunit;

namespace NinjaTools.FluentMockServer.Tests.FluentAPI.Builders.ValueObjects
{
    public class FluentConnectionBuilderTests
    {
        private IFluentConnectionOptionsBuilder CreateSubject()
        {
            return new FluentConnectionOptionsBuilder();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Set_When_KeepAliveOverride(bool keepAliveOverride)
        {
            // Arrange
            var sut = CreateSubject();
            
            // Act
            sut.WithKeepAliveOverride(keepAliveOverride);
            var result = sut.Build();
            
            // Assert
            result.KeepAliveOverride.Should().Be(keepAliveOverride);
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Set_SuppressConnectionHeader(bool suppressConnectionHeader)
        {
            // Arrange
            var sut = CreateSubject();
            
            // Act
            sut.WithSuppressConnectionHeader(suppressConnectionHeader);
            var result = sut.Build();
            
            // Assert
            result.SuppressConnectionHeader.Should().Be(suppressConnectionHeader);
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Set_SuppressContentLengthHeader(bool suppressContentLengthHeader)
        {
            // Arrange
            var sut = CreateSubject();
            
            // Act
            sut.WithSuppressContentLengthHeader(suppressContentLengthHeader);
            var result = sut.Build();
            
            // Assert
            result.SuppressContentLengthHeader.Should().Be(suppressContentLengthHeader);
        }
         
        [Theory]
        [InlineData(1000)]
        [InlineData(0)]
        public void Should_Set_ContentLengthHeaderOverride(long contentLengthHeaderOverride)
        {
            // Arrange
            var sut = CreateSubject();
            
            // Act
            sut.WithContentLengthHeaderOverride(contentLengthHeaderOverride);
            var result = sut.Build();
            
            // Assert
            result.ContentLengthHeaderOverride.Should().Be(contentLengthHeaderOverride);
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Set_CloseSocket(bool closeSocket)
        {
            // Arrange
            var sut = CreateSubject();
            
            // Act
            sut.WithCloseSocket(closeSocket);
            var result = sut.Build();
            
            // Assert
            result.CloseSocket.Should().Be(closeSocket);
        }
    }
}
