using FluentAssertions;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using NinjaTools.FluentMockServer.Xunit;
using NinjaTools.FluentMockServer.Xunit.Attributes;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Xunit
{
    public class MockServerContextTests : XUnitTestBase<MockServerContextTests>, IClassFixture<MockServerFixture>
    {
        [NotNull] public MockServerFixture Fixture { get; }

        /// <inheritdoc />
        public MockServerContextTests([NotNull] MockServerFixture fixture, ITestOutputHelper output) : base(output)
        {
            Fixture = fixture;
        }

        [Fact]
        public void Name_Should_Be_Unique_TestClassTestMethod_Name()
        {
            // Arrange & Act
            var context = Fixture.Register(Output);
            Output.WriteLine($"Name={context.Name};");

            // Assert
            context.Name.Should().Be(XunitContext.Context.UniqueTestName);
        }

        [Fact]
        public void Id_Should_Be_Null_When_No_Context_Specififed()
        {
            // Arrange & Act
            var context = Fixture.Register(Output);
            Output.WriteLine($"Id={context.Id};");

            // Assert
            context.Id.Should().BeNullOrEmpty();
        }

        [Fact]
        [IsolatedMockServerSetup]
        public void Id_Should_Equal_Name_When_Isolated()
        {
            // Arrange & Act
            var context = Fixture.Register(Output);
            Output.WriteLine($"Id={context.Id};");

            // Assert
            context.Id.Should().Be(XunitContext.Context.UniqueTestName);
        }


        [Fact]
        [MockServerCollection("feat(client): concurrent context domains")]
        public void Id_Should_Be_Equal_To_The_CollectionAttribute_Value()
        {
            // Arrange & Act
            var context = Fixture.Register(Output);
            Output.WriteLine($"Id={context.Id};");

            // Assert
            context.Id.Should().Be("feat(client): concurrent context domains");
        }

    }
}

