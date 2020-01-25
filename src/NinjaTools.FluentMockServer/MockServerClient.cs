using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Extensions;
using NinjaTools.FluentMockServer.FluentAPI;
using NinjaTools.FluentMockServer.FluentAPI.Builders;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Utils;
using NinjaTools.FluentMockServer.Xunit;
using static NinjaTools.FluentMockServer.Utils.RequestFactory;

namespace NinjaTools.FluentMockServer
{
    [DebuggerDisplay("Context={Context}; Host={HttpClient.BaseAddress.Host}")]
    public class MockServerClient : IDisposable
    {
        private readonly IMockServerLogger _logger;
        private readonly HttpClient _httpClient;

        [NotNull]
        public HttpClient HttpClient
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Context))
                {
                    _httpClient.DefaultRequestHeaders.Add(HttpExtensions.MockContextHeaderKey, Context);
                }

                return _httpClient;
            }
        }

        public string? Context { get; }

        public MockServerClient([NotNull] HttpClient client, [NotNull] string hostname, [NotNull] IMockServerLogger logger, string? context = null)
        {
            _logger = logger;
            client.BaseAddress = new Uri(hostname);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Host = null;
            _httpClient = client;
            Context = context;
        }

        public MockServerClient([NotNull] string mockServerEndpoint, [NotNull] IMockServerLogger logger, string? context = null) : this(new HttpClient(), mockServerEndpoint, logger, context)
        {
        }

        /// <summary>
        ///     Configures the MockServer Client.
        /// </summary>
        /// <param name="setupFactory"></param>
        /// <returns></returns>
        [NotNull]
        public Task SetupAsync([NotNull] Func<IFluentExpectationBuilder, MockServerSetup> setupFactory)
        {
            var builder = new FluentExpectationBuilder(new MockServerSetup());
            var setup = setupFactory(builder);

            return SetupAsync(setup);
        }


        /// <summary>
        ///     Configures the MockServer Client.
        /// </summary>
        /// <param name="setupFactory"></param>
        /// <returns></returns>
        public Task SetupAsync(Action<IFluentExpectationBuilder> setupFactory)
        {
            var builder = new FluentExpectationBuilder(new MockServerSetup());
            setupFactory(builder);
            var setup = builder.Setup();
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


        [ItemNotNull]
        public async Task<HttpResponseMessage> ResetAsync()
        {
            _logger.WriteLine("Resetting MockServer...");
            var request = GetResetMessage();
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }

        [NotNull]
        public async Task<(bool isValid, string responseMessage)> VerifyAsync([NotNull] Action<IFluentHttpRequestBuilder> verify, [CanBeNull] VerificationTimes times = null)
        {
            var response = await Verify(v =>
            {
                if (times != null)
                {
                    v.Verify(verify).Times(times);
                }
                else
                {
                    v.Verify(verify);
                }
            });
            var responseMessage = await response.Content.ReadAsStringAsync();
            return (response.StatusCode == HttpStatusCode.Accepted, responseMessage);
        }

        [NotNull]
        [ItemNotNull]
        public async Task<HttpResponseMessage> Verify([NotNull] Action<IFluentVerificationBuilder> verify)
        {
            var builder = new FluentVerificationBuilder();
            verify(builder);
            var verification = builder.Build();
            var request = GetVerifyRequest(verification);
            var response = await HttpClient.SendAsync(request);
            return response;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            HttpClient.Dispose();
        }
    }
}
