using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.FluentAPI;
using NinjaTools.FluentMockServer.FluentAPI.Builders;
using NinjaTools.FluentMockServer.Models;
using static NinjaTools.FluentMockServer.Utils.RequestFactory;

namespace NinjaTools.FluentMockServer
{
    public class MockServerClient : IDisposable
    {
        [NotNull]
        public HttpClient HttpClient { get; private set; }

        public MockServerClient(HttpClient client, string hostname = "http://localhost:9003")
        {
            client.BaseAddress = new Uri(hostname);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Host = null;
            HttpClient = client;
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
            var builder = new FluentExpectationBuilder(new MockServerSetup());
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
            foreach (var request in setup.Expectations.Select(GetExpectationMessage))
            {
                var response = await HttpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var response = await HttpClient.SendAsync(request);
            return response;
        }

        public async Task<HttpResponseMessage> ResetAsync()
        {
            var request = GetResetMessage();
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public async Task<HttpResponseMessage> VerifyAsync(Verify verify)
        {
            var request = GetVerifyRequest(verify);
            var response = await HttpClient.SendAsync(request);
            return response;
        }

        public async Task<(bool isValid, string responseMessage)> VerifyAsync(Action<IFluentVerificationBuilder> verify)
        {
            var response = await Verify(verify);
            var responseMessage = await response.Content.ReadAsStringAsync();
            return (response.StatusCode == HttpStatusCode.Accepted, responseMessage);
        }

        public Task<HttpResponseMessage> Verify(Action<IFluentVerificationBuilder> verify)
        {
            var builder = new FluentVerificationBuilder();
            verify(builder);
            var verification = builder.Build();
            return VerifyAsync(verification);
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
            HttpClient?.Dispose();
            HttpClient = null;
        }

    }
}
