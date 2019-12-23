using System;
using FluentAssertions;
using NinjaTools.FluentMockServer.FluentAPI;
using NinjaTools.FluentMockServer.FluentAPI.Builders.ValueObjects;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using Xunit;

namespace NinjaTools.FluentMockServer.Tests.FluentAPI.Builders.ValueObjects
{
    public class FluentDelayBuilderTests
    {

        private IFluentDelayBuilder CreateSubject()
        {
            return new FluentDelayBuilder();
        }
        
        [Theory]
        [InlineData(10, TimeUnit.Seconds)]
        [InlineData(5, TimeUnit.Milliseconds)]
        [InlineData(-20, TimeUnit.Minutes)]
        public void Should_Set_Delay(int value, TimeUnit timeUnit)
        {
            // Arrange
            var sut = CreateSubject();
            
            // Act
            switch (timeUnit)
            {
                case TimeUnit.Nanoseconds:
                case TimeUnit.Microseconds:
                case TimeUnit.Hours:
                case TimeUnit.Days:
                    throw new NotSupportedException($"The {nameof(TimeUnit)} '{timeUnit.ToString()}' s not yet implemented.");
                case TimeUnit.Milliseconds:
                    sut.FromMilliSeconds(value);
                    break;
                case TimeUnit.Seconds:
                    sut.FromSeconds(value);
                    break;
                case TimeUnit.Minutes:
                    sut.FromMinutes(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(timeUnit), timeUnit, null);
            }
            
            var result = sut.Build();
            
            // Assert
            result.Value.Should().Be(value);
            result.TimeUnit.Should().Be(timeUnit);
        }   
    }
}
