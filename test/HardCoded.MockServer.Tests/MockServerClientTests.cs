using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using HardCoded.MockServer.Contracts.Models;
using HardCoded.MockServer.Contracts.Models.ValueTypes;

using Newtonsoft.Json;

using Xunit;
using Xunit.Abstractions;


namespace HardCoded.MockServer.Tests
{
    public class MockHandler : HttpMessageHandler
    {
        private readonly ITestOutputHelper _outputHelper;


        public MockHandler(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }


        public int RequestCounter = 0;
        
        public List<HttpRequestMessage> Requests { get; } = new List<HttpRequestMessage>();
        public List<Expectation> Expectations { get; } = new List<Expectation>();
        
        /// <inheritdoc />
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request);
            var json = await request.Content.ReadAsStringAsync();

            try {
                var content = await request.Content.ReadAsStringAsync();
                var expectation = JsonConvert.DeserializeObject<Expectation>(content);
                
                json = expectation.Serialize();
                Expectations.Add(expectation);
            } 
            catch (Exception e) 
            {
                Console.WriteLine(e);
            } finally {
                _outputHelper.WriteLine($"REQUEST {++RequestCounter}\n{json ?? "some error occurred"}");
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        
    }
    public class FluentExpectationBuilderTests
    {
        private readonly ITestOutputHelper _outputHelper;


        public FluentExpectationBuilderTests(ITestOutputHelper outputHelper) { _outputHelper = outputHelper; }
        
        
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
                            .RespondWith(HttpStatusCode.Accepted, response => response.WithDelay(10, TimeUnit.SECONDS))
                            .Setup());

            // Assert
            handler.Expectations.Should().ContainSingle(
                e =>
                            e.HttpRequest.Path            == "/some/path"
                         && e.HttpRequest.Method          == "POST"
                         && e.HttpRequest.Secure          == true
                         && e.HttpResponse.Delay.Value    == 10
                         && e.HttpResponse.Delay.TimeUnit == TimeUnit.SECONDS);
            
        }
    }
}
