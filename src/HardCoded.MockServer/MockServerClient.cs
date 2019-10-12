using System;
using System.Net.Http;
using System.Threading.Tasks;
using HardCoded.MockServer.Models;
using HardCoded.MockServer.Requests;

namespace HardCoded.MockServer
{
    public class MockServerClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        public Uri MockServerEndpoint { get; }

        public MockServerClient(string mockServerEndpoint) : this(new Uri(mockServerEndpoint))
        {
        }
        
        public MockServerClient(Uri mockServerUri)
        {
            MockServerEndpoint = mockServerUri;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = mockServerUri;
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Host = null;
        }

        public async Task<HttpResponseMessage> SetupExpectationAsync(ExpectationRequest request)
        {
            return await _httpClient.SendAsync(request);
        }
        
        public async Task<HttpResponseMessage> SetupExpectationAsync(params Expectation[] expectations)
        {
            var request = new ExpectationRequest();
            request.AddRange(expectations);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var response = await _httpClient.SendAsync(request);
            return response;
        }

        public async Task<HttpResponseMessage> Reset()
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("reset", UriKind.Relative)
            };
            
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
    }
}