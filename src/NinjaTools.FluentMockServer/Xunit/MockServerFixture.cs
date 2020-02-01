using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Xunit
{
    /// <inheritdoc cref="IAsyncLifetime" />
    /// <summary>
    /// An <see cref="T:Xunit.IClassFixture`1" /> exposing a <see cref="T:NinjaTools.FluentMockServer.MockServerClient" />.
    /// </summary>
    [PublicAPI]
    public class MockServerFixture : IDisposable, IAsyncLifetime
    {
        [NotNull]
        public MockServerContext Context { get; private set; }

        [NotNull]
        public MockServerContext Register(ITestOutputHelper testOutputHelper, [CallerFilePath] string? sources = null)
        {
            XunitContext.Register(testOutputHelper);
            Context = MockServerContext.Establish();
            return Context;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Context.MockClient.Dispose();
            MockServerContext.Container.Dispose();
        }

        /// <inheritdoc />
        [NotNull]
        public Task InitializeAsync()
        {
            return MockServerContext.Container.StartAsync();
        }

        /// <inheritdoc />
        public async Task DisposeAsync()
        {
            await Context.MockClient.ResetAsync();
            MockServerContext.Container.Dispose();
        }

    }
}
