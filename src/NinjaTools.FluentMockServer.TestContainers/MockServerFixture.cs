using System;
using System.Threading.Tasks;
using Xunit;

namespace NinjaTools.FluentMockServer.TestContainers
{
    /// <summary>
    /// A TestFixture exposing a <see cref="MockServerClient"/>.
    /// </summary>
    public class MockServerFixture : IDisposable, IAsyncLifetime
    {
        /// <summary>
        /// The MockServerClient
        /// </summary>
        public MockServerClient Client { get; }
        
        /// <summary>
        /// Handle to the <see cref="MockServerContainer"/>.
        /// </summary>
        protected MockServerContainer Container { get; }
         
        public MockServerFixture()
        {
            Container = new MockServerContainer();
            Client = new MockServerClient(Container.MockServerBaseUrl);
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
            Client.Dispose();
            return Container.StopAsync();
        }
    }
}
