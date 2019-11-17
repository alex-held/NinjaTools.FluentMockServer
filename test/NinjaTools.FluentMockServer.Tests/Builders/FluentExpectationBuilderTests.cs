using System;
using System.Linq;
using System.Net;
using System.Net.Http;

using FluentAssertions;
using NinjaTools.FluentMockServer.Builders.Expectation;
using NinjaTools.FluentMockServer.Models.ValueTypes;

using Xunit;
using Xunit.Abstractions;


namespace NinjaTools.FluentMockServer.Tests.Builders
{
    public class FluentExpectationBuilderTests
    {
        private readonly ITestOutputHelper _outputHelper;
        public FluentExpectationBuilderTests(ITestOutputHelper outputHelper) { _outputHelper = outputHelper; }
        
        
        [Fact]
        public void Should_Set_Times()
        {
            // Arrange
            Action<IFluentExpectationBuilder> factory = builder => builder
                        .OnHandling(HttpMethod.Post, request => request.WithPath("/"))
                        .RespondOnce(HttpStatusCode.Created, resp => resp.WithDelay(1, TimeUnit.Milliseconds));
            
            var setup = new MockServerSetup();
            var builder = new FluentExpectationBuilder(setup);
            
            // Act
            factory(builder);
            var expectation = builder.Setup().Expectations.First();
            var result = expectation.ToString();
            
            // Assert
            _outputHelper.WriteLine(result);
            result.Should()
                    .MatchRegex(@"(?m)\s*""times"":\s*\{\s*""remainingTimes"":\s*1,\s*""unlimited"":\s*false\s*}");
        }
        
        [Theory]
        [InlineData(10, "false")]
        [InlineData(0, "true")]
        public void Should_Set_Times_2(int times, string unlimited)
        {
            // Arrange
            var builder = new FluentExpectationBuilder();
            
            // Act
            var result = builder.RespondTimes(times, 200).Setup().Expectations.First().ToString();
            
            
            // Assert
            _outputHelper.WriteLine(result);
            result
                .Should()
                .MatchRegex($@"(?m)\s*""times"":\s*\{{\s*""remainingTimes"":\s*{times},\s*""unlimited"":\s*{unlimited}\s*}}");
        }
        
        [Fact]
        public void Should_Set_TimeToLive()
        {
            // Arrange
            var builder = new FluentExpectationBuilder();
            
            // Act
            var result = builder
                .OnHandlingAny()
                .RespondWith(HttpStatusCode.OK)
                .WhichIsValidFor(10, TimeUnit.Seconds)
                .Setup()
                .Expectations.First()
                .ToString();
            
            // Assert
            _outputHelper.WriteLine(result);
            result.Should().MatchRegex(@"(?m)\s*""timeToLive"":\s*\{\s*""timeUnit"":\s*""SECONDS""\s*,\s*""timeToLive"":\s*10\s*,\s*""unlimited""\s*:\s*false\s*}");
        }

        
        [Fact]
        public void Should_Match_Any_Request()
        {
            // Arrange
            Action<IFluentExpectationBuilder> factory = builder => builder
                        .OnHandlingAny()
                        .RespondWith(HttpStatusCode.Created);
            
            var setup = new MockServerSetup();
            var builder = new FluentExpectationBuilder(setup);
            
            // Act
            factory(builder);
            var expectation = builder.Setup().Expectations.First();
            var result = expectation.ToString();
            
            // Assert
            _outputHelper.WriteLine(result);
            result.Should().MatchRegex(@"(?s)^((?!httpRequest).)*$");
        }
        
        [Theory]
        [InlineData("POST")]
        [InlineData("GET")]
        [InlineData("PUT")]
        public void Should_Match_Any_Request_With_Method(string method)
        {
            // Arrange
            Action<IFluentExpectationBuilder> factory = builder => builder
                        .OnHandling(new HttpMethod(method))
                        .RespondWith(HttpStatusCode.Created);
            
            var setup = new MockServerSetup();
            var builder = new FluentExpectationBuilder(setup);
            
            // Act
            factory(builder);
            var expectation = builder.Setup().Expectations.First();
            var result = expectation.ToString();
            
            // Assert
            _outputHelper.WriteLine(result);
            result.Should().MatchRegex($@"(?smi)""httpRequest"":.*{{.*""method"".*:.*""{method}"".*}}.*,");
        }
    }
    
}

