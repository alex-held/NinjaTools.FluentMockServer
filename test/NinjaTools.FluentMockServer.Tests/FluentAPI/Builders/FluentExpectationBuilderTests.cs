using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.Extensions;
using NinjaTools.FluentMockServer.FluentAPI;
using NinjaTools.FluentMockServer.FluentAPI.Builders;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Tests.TestHelpers.Mocks;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.FluentAPI.Builders
{
    public class FluentExpectationBuilderTests
    {
        private readonly ITestOutputHelper _outputHelper;
        public FluentExpectationBuilderTests(ITestOutputHelper outputHelper) { _outputHelper = outputHelper; }
        
        
        [Fact]
        public void Should_Set_Times()
        {
            // Arrange
            Action<IFluentExpectationBuilder> fac = expectationBuilder => expectationBuilder
                        .OnHandling(HttpMethod.Post, request => request.WithPath("/"))
                        .RespondOnce(HttpStatusCode.Created, resp => resp.WithDelay(1, TimeUnit.Milliseconds));
            
            
            var setup = new MockServerSetup();
            var builder = new FluentExpectationBuilder(setup);
            
            // Act
            fac(builder);
            var expectation = builder.Setup().Expectations.First();
            var result = expectation.AsJson(); 
            
            // Assert
            _outputHelper.WriteLine(result);
            result.Should()
                    .MatchRegex(@"(?m)\s*""times"":\s*\{\s*""remainingTimes"":\s*1,\s*""unlimited"":\s*false\s*}");
        }
        
        [Fact]
        public void Should_Set_BaseUrl()
        {
            // Arrange
            const string baseUrl = "http://example.com";
            var builder = new FluentExpectationBuilder();
            builder.WithBaseUrl(baseUrl);
            
            // Act
            var setup = builder.Setup();
            
            // Assert
            _outputHelper.WriteLine(JsonConvert.SerializeObject(setup, Formatting.Indented));
            setup.BaseUrl.Should().Be(baseUrl);
        }
        
        [Fact]
        public void Should_Set_Times_Always()
        {
            // Arrange
            var builder = new FluentExpectationBuilder();
            var setup = builder
                .RespondTimes(() => Times.Always,200)
                .Setup();
            
            // Act
            var result =   setup .Expectations.First().AsJson();
            
            // Assert
            _outputHelper.WriteLine(result);
            result
                .Should()
                .MatchRegex($@"(?m)\s*""times"":\s*\{{\s*""remainingTimes"":\s*0,\s*""unlimited"":\s*true\s*}}");
        }
        
        [Theory]
        [InlineData(10)]
        [InlineData(5)]
        public void Should_Set_Times_Limited(int times)
        {
            // Arrange
            var builder = new FluentExpectationBuilder();
            
            // Act
            var result = builder.RespondTimes(times, 200).Setup().Expectations.First().AsJson();
            
            // Assert
            _outputHelper.WriteLine(result);
            result
                .Should()
                .MatchRegex($@"(?m)\s*""times"":\s*\{{\s*""remainingTimes"":\s*{times},\s*""unlimited"":\s*false\s*}}");
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
                .AsJson();
            
            // Assert
            _outputHelper.WriteLine(result);
            result.Should().MatchRegex(@"(?m)\s*""timeToLive"":\s*\{\s*""timeUnit"":\s*""SECONDS""\s*,\s*""timeToLive"":\s*10\s*,\s*""unlimited""\s*:\s*false\s*}");
        }

        
        [Fact]
        public void Should_Match_Any_Request()
        {
            // Arrange
            var builder = new FluentExpectationBuilder();
            builder
                .OnHandlingAny()
                .RespondWith(HttpStatusCode.Created);
            
            // Act
            var expectation = builder.Setup().Expectations.First();
            var result = expectation.AsJson();
            
            // Assert
            _outputHelper.WriteLine(result);
            result.Should().MatchRegex(@"(?s)^((?!httpRequest).)*$");
        }
        
        [Fact]
        public void Should_Match_Any_Request_With_HttpMethod()
        {
            // Arrange
            var httpMethod = HttpMethod.Post;
            var builder = new FluentExpectationBuilder();
            builder
                .OnHandlingAny(httpMethod)
                .RespondWith(HttpStatusCode.Created);
            
            // Act
            var expectation = builder.Setup().Expectations.First();
            var result = expectation.AsJson();
            
            // Assert
            _outputHelper.WriteLine(result);
            result.Should().MatchRegex($@"(?smi)""httpRequest"":.*{{.*""method"".*:.*""{httpMethod.Method}"".*}}.*,");
        }
        
        [Theory]
        [InlineData("POST")]
        [InlineData("GET")]
        [InlineData("PUT")]
        public void Should_Match_Any_Request_With_Method(string method)
        {
            // Arrange
            var builder = new FluentExpectationBuilder();
            
            // Act
            builder.OnHandling(new HttpMethod(method))
                .RespondWith(HttpStatusCode.Created);
            var expectation = builder.Setup().Expectations.First();
            var result = expectation.AsJson();
            
            // Assert
            _outputHelper.WriteLine(result);
            result.Should().MatchRegex($@"(?smi)""httpRequest"":.*{{.*""method"".*:.*""{method}"".*}}.*,");
        }
        
        [Fact]
        public async Task Should_Build_Expectation()
        {
            // Arrange
            var handler = new MockHandler(_outputHelper);
            var mockServerClient = new MockServerClient(new HttpClient(handler));
            
            // Act
            await mockServerClient.SetupAsync(
                builder => builder
                    .OnHandling(HttpMethod.Post, request => request.WithPath("some/path").EnableEncryption())
                    .RespondWith(HttpStatusCode.Accepted, response => response.WithDelay(10, TimeUnit.Seconds))
                    .Setup());

            // Assert
            handler.Expectations.Should().ContainSingle(
                e =>
                    e.HttpRequest.Path            == "/some/path"
                    && e.HttpRequest.Method          == "POST"
                    && e.HttpRequest.Secure          == true
                    && e.HttpResponse.Delay.Value    == 10
                    && e.HttpResponse.Delay.TimeUnit == TimeUnit.Seconds);
            
        }
    }
    
}

