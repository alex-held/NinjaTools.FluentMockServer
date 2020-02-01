using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.Exceptions;
using NinjaTools.FluentMockServer.Extensions;
using NinjaTools.FluentMockServer.FluentAPI;
using NinjaTools.FluentMockServer.FluentAPI.Builders;
using NinjaTools.FluentMockServer.FluentAPI.Builders.HttpEntities;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Serialization;
using NinjaTools.FluentMockServer.Utils;
using static NinjaTools.FluentMockServer.Utils.RequestFactory;

namespace NinjaTools.FluentMockServer
{
    /// <inheritdoc />
    /// <summary>
    ///     Provides a base class for interacting with a MockServer.
    ///     Following operations are supported:
    ///     <list type="bullet">
    ///         <item>
    ///             <description> Create an <see cref="T:NinjaTools.FluentMockServer.Models.Expectation" />. </description>
    ///         </item>
    ///         <item>
    ///             <description> Verify whether the MockServer received a matching <see cref="T:NinjaTools.FluentMockServer.Models.HttpEntities.HttpRequest" />. </description>
    ///         </item>
    ///     </list>
    ///     setting up <see cref="T:NinjaTools.FluentMockServer.Models.Expectation" />  HTTP requests and receiving HTTP responses from a resource identified by a URI.
    /// </summary>
    [DebuggerDisplay("Context={Context}; Host={HttpClient.BaseAddress.Host}")]
    [PublicAPI]
    public class MockServerClient : IDisposable
    {
        private readonly IMockServerLogger _logger;

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

        private readonly HttpClient _httpClient;

        public string? Context { get; internal set; }

        public MockServerClient([NotNull] HttpClient client, [NotNull] string hostname, [NotNull] IMockServerLogger logger, string? context = null)
        {
            _logger = logger;
            client.BaseAddress = new Uri(hostname);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Host = null;
            _httpClient = client;
            Context = context;
        }

        /// <inheritdoc />
        public MockServerClient([NotNull] string mockServerEndpoint, [NotNull] IMockServerLogger logger, string? context = null)
            : this(new HttpClient(), mockServerEndpoint, logger, context)
        {
        }


        /// <summary>
        ///     Configures the MockServer Client.
        /// </summary>
        /// <param name="setupFactory"> </param>
        /// <returns> </returns>
        [NotNull]
        public Task SetupAsync([NotNull] Func<IFluentExpectationBuilder, MockServerSetup> setupFactory)
        {
            var builder = new FluentExpectationBuilder(new MockServerSetup());
            var setup = setupFactory(builder);

            return SetupInternal(setup);
        }


        /// <summary>
        ///     Configures the MockServer Client.
        /// </summary>
        /// <param name="setupFactory"> </param>
        public Task SetupAsync(Action<IFluentExpectationBuilder> setupFactory)
        {
            var builder = new FluentExpectationBuilder(new MockServerSetup());
            setupFactory(builder);
            var setup = builder.Setup();
            return SetupInternal(setup);
        }


        /// <summary>
        ///     Configures the MockServer Client using a predefined <see cref="MockServerSetup" />.
        /// </summary>
        /// <exception cref="MockServerOperationFailedException"></exception>
        /// <param name="setup"> </param>
        private async Task SetupInternal(MockServerSetup setup)
        {
            foreach (var request in setup.Expectations.Select(GetExpectationMessage))
            {
                var response = await HttpClient.SendAsync(request);
                response.EnsureSuccessfulMockServerOperation();
            }
        }


        /// <summary>
        /// Resets all <see cref="Expectation"/> on the MockServer.
        /// </summary>
        /// <exception cref="MockServerOperationFailedException"></exception>
        public async Task ResetAsync()
        {
            _logger.WriteLine("Resetting MockServer...");
            var request = GetResetMessage();
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessfulMockServerOperation();
        }

        /// <summary>
        /// Verifies that a the MockServer received a matching <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="verify">Configure the matcher for the <see cref="HttpRequest"/>.</param>
        /// <param name="times">Configure how many <see cref="VerificationTimes"/> the matched request should have occured.</param>
        /// <exception cref="MockServerVerificationException"></exception>
        [NotNull]
        public Task VerifyAsync([NotNull] Action<IFluentHttpRequestBuilder> verify, [CanBeNull] VerificationTimes times = null)
        {
            return times is null ? VerifyAsync(v => v.Verify(verify)) : VerifyAsync(v => v.Verify(verify).Times(times));
        }

        /// <summary>
        /// Verifies that a the MockServer received a matching <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="verify">Configure the matcher for the <see cref="HttpRequest"/>.</param>
        /// <exception cref="MockServerVerificationException"></exception>
        [NotNull]
        public async Task VerifyAsync([NotNull] Action<IFluentVerificationBuilder> verify)
        {
            var builder = new FluentVerificationBuilder();
            verify(builder);
            var verification = builder.Build();
            await VerifyInternal(verification);
        }

        private async Task VerifyInternal(Verify verify)
        {
            var request = GetVerifyRequest(verify);
            var response = await HttpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                var responseMessage = await response.Content.ReadAsStringAsync();
                throw new MockServerVerificationException(responseMessage, verify.HttpRequest);
            }
        }

        /// <summary>
        /// Retrieves a list of active <see cref="Expectation"/> from the MockServer.
        /// <exception cref="MockServerOperationFailedException" />
        /// </summary>
        [PublicAPI]
        [ItemNotNull]
        public async Task<IReadOnlyList<Expectation>> ListSetupsAsync()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, "mockserver/retrieve?type=ACTIVE_EXPECTATIONS");
            var response = await HttpClient.SendAsync(requestMessage);
            response.EnsureSuccessfulMockServerOperation();
            var stringContent = await response.Content.ReadAsStringAsync();
            var setups = Serializer.Deserialize<List<Expectation>>(stringContent);
            return setups;
        }


        /// <inheritdoc />
        public void Dispose()
        {
            HttpClient.Dispose();
        }

    }
}
