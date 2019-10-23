using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using HardCoded.MockServer.Contracts.Extensions;
using HardCoded.MockServer.Contracts.Models;
using HardCoded.MockServer.Contracts.Serialization;
using HardCoded.MockServer.Fluent;
using HardCoded.MockServer.Fluent.Builder.Expectation;
using HardCoded.MockServer.Requests;

namespace HardCoded.MockServer
{
    public class MockServerClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        public Uri MockServerEndpoint => _httpClient.BaseAddress;


        internal MockServerClient(HttpClient httpClient)
        {
            _httpClient = httpClient.WithDefaults(new Uri("http://localhost:1080"));
        }
        
        public MockServerClient(string mockServerEndpoint) : this(new Uri(mockServerEndpoint))
        { }
        
        public MockServerClient(Uri mockServerUri)
        {
            _httpClient = new HttpClient().WithDefaults(mockServerUri);
        }

      
        public async Task<HttpResponseMessage> SetupExpectationAsync(ExpectationRequest request)
        {
            return await _httpClient.SendAsync(request);
        }


        public async Task SetupAsync(Func<IFluentExpectationBuilder, MockServerSetup> setupFactory )
        {
            var builder = new FluentExpectationBuilder(new MockServerSetup());
            var setup = setupFactory(builder);
            
            foreach ( var expectation in setup.Expectations ) {
                var request = new HttpRequestMessage(HttpMethod.Put, GetMockServerUri("expectation"))
                {
                            Content = new JsonContent(expectation)
                };
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
        }
        
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            request.RequestUri = new Uri(MockServerEndpoint, request.RequestUri);
        
            var response = await _httpClient.SendAsync(request);
            return response;
        }

        public async Task<HttpResponseMessage> Reset()
        {
            var request = new HttpRequestMessage(HttpMethod.Put, GetMockServerUri("reset"));
  
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Reset().GetAwaiter().GetResult();
            _httpClient?.Dispose();
        }

        public async Task<HttpResponseMessage> Verify(VerificaionRequest request)
        {
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }

        private protected Uri GetMockServerUri(string path) => new Uri(MockServerEndpoint, $"mockserver/{path}");
    }
}
