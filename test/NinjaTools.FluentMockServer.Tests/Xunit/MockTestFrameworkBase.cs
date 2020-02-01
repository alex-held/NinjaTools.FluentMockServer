using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Xunit;
using NinjaTools.FluentMockServer.Xunit.Attributes;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Xunit
{
    public class MockTestFrameworkTests : MockServerTestBase, IDisposable
    {
        public MockTestFrameworkTests([NotNull] MockServerFixture fixture, ITestOutputHelper outputHelper) : base(fixture, outputHelper)
        { }

        [Fact]
        public void Should_Discover()
        {
            Output.WriteLine("Discovered!");
        }


        [Fact]
        [IsolatedMockServerSetup]
        public async Task Should_Set_ContextHeader_On_HttpClient_Requests()
        {
            // Act
            var result = await HttpClient.GetAsync("http://google.com");

            // Assert
            if(result.RequestMessage.TryGetMockContextHeader(out var context))
            {
                context.Should().Contain(Context.Id);
            }
        }

        [Fact]
        [IsolatedMockServerSetup]
        public void Should_Discover_Isolate()
        {
            // Assert
            ContextRegistry.Instance.Isolated.Count(m => m == XunitContext.Context.MethodInfo).Should().Be(1);
        }

        [Fact]
        [IsolatedMockServerSetup]
        public void Should_Discover_Set_Unique_Id()
        {
            // Arrange
            Context.Id.Should().NotBeNullOrWhiteSpace();
        }


        [Fact]
        [IsolatedMockServerSetup]
        public async Task Should_Set_Unique_Id_In_Header()
        {
            // Arrange
            Output.WriteLine($"Id = {Context.Id}");

            // Act
            var response = await Context.HttpClient.GetAsync("https://google.com");

            // Assert
            response.RequestMessage.TryGetMockContextHeader(out var contextId).Should().BeTrue();
            contextId.Should().Be(Context.Id);
        }

        [Fact]
        public void Should_Invoke_GlobalSetup()
        {
            GlobalSetup.Called.Should().BeTrue();
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}
