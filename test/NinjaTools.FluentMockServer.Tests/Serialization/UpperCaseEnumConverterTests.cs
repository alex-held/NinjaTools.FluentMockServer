using FluentAssertions;

using Newtonsoft.Json;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using Xunit;
using Xunit.Abstractions;


namespace NinjaTools.FluentMockServer.Tests.Serialization
{
    public class UpperCaseEnumConverterTests
    {

        private readonly ITestOutputHelper _outputHelper;


        public UpperCaseEnumConverterTests(ITestOutputHelper outputHelper) { _outputHelper = outputHelper; }


        [Theory]
        [InlineData(TimeUnit.Nanoseconds, "NANOSECONDS")]
        [InlineData(TimeUnit.Microseconds, "MICROSECONDS")]
        [InlineData(TimeUnit.Milliseconds, "MILLISECONDS")]
        [InlineData(TimeUnit.Seconds, "SECONDS")]
        [InlineData(TimeUnit.Hours, "HOURS")]
        [InlineData(TimeUnit.Days, "DAYS")]
        public void Should_TESTNAME(TimeUnit timeUnit, string expected)
        {
            // Act
            var result = JsonConvert.SerializeObject(timeUnit);
            _outputHelper.WriteLine(result);

            // Assert
            result.Should().Be( $@"""{expected}""");
        }
    }
}
