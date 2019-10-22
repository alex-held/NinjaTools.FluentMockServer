using System;
using System.Collections.Generic;
using System.Linq;
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


namespace HardCoded.MockServer.Fluent.Tests.Builders
{
    public class MockHandler : HttpMessageHandler
    {

        
        public List<HttpRequestMessage> Requests { get; } = new List<HttpRequestMessage>();
        public List<Expectation> Expectations { get; } = new List<Expectation>();
        
        /// <inheritdoc />
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request);

            try {
                var content = await request.Content.ReadAsStringAsync();
                var expectation = JsonConvert.DeserializeObject<List<Expectation>>(content);
                Expectations.AddRange(expectation);
            } catch (Exception e) {
                Console.WriteLine(e);
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
            var setup = MockServerBootstrap.Expectations
                        .OnHandling(HttpMethod.Post, request =>  request.EnableEncryption().WithPath("some/path"))
                        .RespondWith(HttpStatusCode.Accepted, response => response.WithDelay(10, TimeUnit.SECONDS))
                        .Setup();
            // Act

            var handler = new MockHandler();
            var httpClient = new HttpClient(handler);
            var mockServerClient = new MockServerClient(httpClient);

            await mockServerClient.SetupAsync(builder => builder
                            .OnHandling(HttpMethod.Post, request => request.WithPath("some/path").EnableEncryption())
                            .RespondWith(HttpStatusCode.Accepted, response => response.WithDelay(10, TimeUnit.SECONDS))
                            .Setup());

            // Assert
            var request = handler.Requests.First();

            handler.Expectations.Single(
                e => e.HttpRequest.Path == "some/path" && e.HttpRequest.Secure          == true
                                                       && e.HttpResponse.Delay.Value    == 10
                                                       && e.HttpResponse.Delay.TimeUnit == TimeUnit.SECONDS)
                        .Should().NotBeNull();
            
           _outputHelper.WriteLine("Send:\n" + request.Content.ReadAsStringAsync().Result);
           _outputHelper.WriteLine("Expected:\n" + JsonConvert.SerializeObject(setup.Expectations.First(), Formatting.Indented));
        }
    }
}
