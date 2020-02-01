using System;
using System.Net.Http;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using NinjaTools.FluentMockServer.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Xunit
{
    public abstract class MockServerTestBase : XUnitTestBase, IDisposable, IClassFixture<MockServerFixture>
    {
        public MockServerClient MockClient  => Context.MockClient;
        public HttpClient HttpClient => MockClient.HttpClient;
        public MockServerContext Context { get; }
        /// <inheritdoc />
        protected MockServerTestBase(MockServerFixture fixture, ITestOutputHelper output) : base(output)
        {
            Context =  fixture.Register(output);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            MockClient.ResetAsync().GetAwaiter().GetResult();
        }
    }
}
