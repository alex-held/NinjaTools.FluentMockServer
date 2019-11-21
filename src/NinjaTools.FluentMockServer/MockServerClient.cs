using System;
using System.Net.Http;
using System.Threading.Tasks;
using NinjaTools.FluentMockServer.Builders.Expectation;
using NinjaTools.FluentMockServer.Extensions;
using NinjaTools.FluentMockServer.Requests;
using NinjaTools.FluentMockServer.Serialization;
using NinjaTools.FluentMockServer.Utils;


namespace NinjaTools.FluentMockServer
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
        

        /// <summary>
        /// Configures the MockServer Client.
        /// </summary>
        /// <param name="setupFactory"></param>
        /// <returns></returns>
        public async Task SetupAsync(Func<IFluentExpectationBuilder, MockServerSetup> setupFactory)
        {
            var builder = new FluentExpectationBuilder(new MockServerSetup());
            var setup = setupFactory(builder);
            
            if (setup.BaseUrl != null)
            { 
                var uri = new Uri(MockServerEndpoint, setup.BaseUrl);
                _httpClient.BaseAddress = uri;
            }
 
            foreach ( var expectation in setup.Expectations )
            {
                var request = new HttpRequestMessage(HttpMethod.Put, GetMockServerUri("expectation"))
                {
                            Content = new JsonContent(expectation)
                };
                
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
        }
        
        
        /// <summary>
        /// Configures the MockServer Client using a predefined <see cref="MockServerSetup"/>.
        /// </summary>
        /// <param name="setup"></param>
        /// <returns></returns>
        public async Task SetupAsync(MockServerSetup setup)
        {
            if (setup.BaseUrl != null)
            { 
                var uri = new Uri(MockServerEndpoint, setup.BaseUrl);
                _httpClient.BaseAddress = uri;
            }
            
            foreach ( var expectation in setup.Expectations ) 
            {
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

        private Uri GetMockServerUri(string path)
        {
            return new Uri(MockServerEndpoint, $"mockserver/{path}");
        }
    }
}
