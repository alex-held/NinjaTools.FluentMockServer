using System;
using System.Net.Http;
using System.Reflection;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Utils;
using Xunit;

namespace NinjaTools.FluentMockServer.Xunit
{
    public class MockServerContext
    {
        [NotNull]
        public static MockServerContext Establish()
        {
            var context = new MockServerContext();

            if (ContextRegistry.Instance.TryGetIdentifier(context.TestMethod, out var id))
            {
                context.MockClient  = new MockServerClient(Container.MockServerBaseUrl, MockServerTestLogger.Instance, id);
                return context;
            }

            context.MockClient  = new MockServerClient(Container.MockServerBaseUrl, MockServerTestLogger.Instance);
            return context;
        }

        /// <summary>
        /// Gets the <see cref="MockServerClient"/>.
        /// </summary>
        [NotNull]
        public MockServerClient MockClient { get; private set; }

        [CanBeNull]
        public string? Id => MockClient.Context;

        [NotNull]
        public string Name => XunitContext.Context.UniqueTestName;

        [NotNull]
        public MethodInfo TestMethod => XunitContext.Context.MethodInfo;

        /// <summary>
        /// Handle to the <see cref="MockServerContainer"/>.
        /// </summary>
        [NotNull]
        internal static MockServerContainer Container => _container.Value;

        /// <summary>
        /// Gets the <see cref="System.Net.Http.HttpClient"/> of the <see cref="MockClient"/>.
        /// </summary>
        [NotNull]
        public HttpClient HttpClient => MockClient.HttpClient;

        /// <summary>
        /// The port used to connect the <see cref="MockClient"/>.
        /// </summary>
        public int MockServerPort => Container.HostPort;

        /// <summary>
        /// The host used to connect the <see cref="MockClient"/>.
        /// </summary>
        [NotNull]
        public string MockServerBaseUrl => Container.MockServerBaseUrl;

        private static readonly Lazy<MockServerContainer> _container = new Lazy<MockServerContainer>(() => new MockServerContainer());
    }
}
