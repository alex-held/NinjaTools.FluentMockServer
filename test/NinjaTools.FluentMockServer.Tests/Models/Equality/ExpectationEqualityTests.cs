using FluentAssertions;
using NinjaTools.FluentMockServer.FluentAPI.Builders;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Models.Equality
{
    public class ExpectationEqualityTests : EqualityTestBase<Expectation>
    {
        [Fact]
        public void Equals_Should_Be_False_When_Instances_Are_Not_Equal()
        {
            // Arrange
            var a = FluentExpectationBuilder.Create(times: Times.Once);
            var b = FluentExpectationBuilder.Create();
            
            // Assert
            a.Should().NotBe(b);
        }
        
        /// <inheritdoc />
        public ExpectationEqualityTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }
    }
}
