using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Intrinsics;
using System.Text.RegularExpressions;

using FluentAssertions;

using HardCoded.MockServer.Contracts.Models.ValueTypes;
using HardCoded.MockServer.Fluent.Builder.Expectation;

using Xunit;
using Xunit.Abstractions;


namespace HardCoded.MockServer.Fluent.Tests.Builders
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
                        .RespondOnce(HttpStatusCode.Created, resp => resp.WithDelay(1, TimeUnit.MILLISECONDS));
            
            var setup = new MockServerSetup();
            var builder = new FluentExpectationBuilder(setup);
            
            // Act
            factory(builder);
            var expectation = builder.Setup().Expectations.First();
            var result = expectation.Serialize();
            
            // Assert
            _outputHelper.WriteLine(result);
            result.Should()
                    .MatchRegex(@"(?m)\s*""times"":\s*\{\s*""remainingTimes"":\s*1,\s*""unlimited"":\s*false\s*}");
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
                        .WhichIsValidFor(10)
                        .Setup()
                        .Expectations.First()
                        .Serialize();
            
            // Assert
            _outputHelper.WriteLine(result);
            result.Should().MatchRegex(@"(?m)\s*""timeToLive"":\s*\{\s*""time"":\s*10,\s*""timeUnit"":\s*""SECONDS""\s*}");
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
            var result = expectation.Serialize();
            
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
            var result = expectation.Serialize();
            
            // Assert
            _outputHelper.WriteLine(result);
            result.Should().MatchRegex($@"(?smi)""httpRequest"":.*{{.*""method"".*:.*""{method}"".*}}.*,");
        }
    }
    
}

