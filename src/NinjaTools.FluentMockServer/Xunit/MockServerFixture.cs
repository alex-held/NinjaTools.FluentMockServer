using System;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Utils;
using Xunit;

namespace NinjaTools.FluentMockServer.Xunit
{
    /// <summary>
    /// An <see cref="IClassFixture{MockServerFixture}"/> exposing a <see cref="MockServerClient"/>.
    /// </summary>
    public class MockServerFixture : IDisposable, IAsyncLifetime
    {
        /// <summary>
        /// Gets the <see cref="MockServerClient"/>.
        /// </summary>
        [NotNull]
        public MockServerClient MockClient { get; }

        /// <summary>
        /// Gets the <see cref="System.Net.Http.HttpClient"/> of the <see cref="MockClient"/>.
        /// </summary>

        public HttpClient HttpClient => MockClient.HttpClient;

        /// <summary>
        /// The port used to connect the <see cref="MockClient"/>.
        /// </summary>
        public int MockServerPort => Container.HostPort;

        /// <summary>
        /// The host used to connect the <see cref="MockClient"/>.
        /// </summary>
        public string MockServerBaseUrl => Container.MockServerBaseUrl;

        /// <summary>
        /// Handle to the <see cref="MockServerContainer"/>.
        /// </summary>
        [NotNull]
        protected MockServerContainer Container { get; }
         
        /// <summary>
        ///
        /// </summary>
        public MockServerFixture()
        {
            Container = new MockServerContainer();
            MockClient = new MockServerClient(Container.MockServerBaseUrl, MockServerTestLogger.Instance);
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
           DisposeAsync().GetAwaiter().GetResult();
        }

        /// <inheritdoc />
        public Task InitializeAsync()
        {
          return Container.StartAsync();
        }

        /// <inheritdoc />
        public Task DisposeAsync()
        {
            MockClient.Dispose();
            return Container.StopAsync();
        }
    }
}
