using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NinjaTools.FluentMockServer.Domain.Builders.Expectation;
using NinjaTools.FluentMockServer.Domain.Extensions;
using NinjaTools.FluentMockServer.Domain.Models;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;
using static NinjaTools.FluentMockServer.Domain.Utils.RequestFactory;

namespace NinjaTools.FluentMockServer.Domain
{
    public class MockServerClient : IDisposable
    {
        private HttpClient _httpClient;

        public MockServerClient(HttpClient client, string hostname = "http://localhost:9003")
        {
            _httpClient = client.WithDefaults(new Uri(hostname));
        }
        
        public MockServerClient(string mockServerEndpoint) : this(new HttpClient(),mockServerEndpoint)
        {
        }

        
        /// <summary>
        ///     Configures the MockServer Client.
        /// </summary>
        /// <param name="setupFactory"></param>
        /// <returns></returns>
        public Task SetupAsync(Func<IFluentExpectationBuilder, MockServerSetup> setupFactory)
        {
            var builder = new Expectation.FluentExpectationBuilder(new MockServerSetup());
            var setup = setupFactory(builder);

            return SetupAsync(setup);
        }


        /// <summary>
        ///     Configures the MockServer Client using a predefined <see cref="MockServerSetup" />.
        /// </summary>
        /// <param name="setup"></param>
        /// <returns></returns>
        public async Task SetupAsync(MockServerSetup setup)
        {
            foreach (var request in setup.Expectations.Select(Expectation))
            {
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var response = await _httpClient.SendAsync(request);
            return response;
        }

        public async Task<HttpResponseMessage> ResetAsync()
        {
            var request = Reset();
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public async Task<HttpResponseMessage> VerifyAsync(Verify verify)
        {
            var request = Verify(verify);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }
        
        
        /// <inheritdoc />
        public void Dispose()
        {
            _httpClient?.Dispose();
            _httpClient = null;
        }

    }
}
