using System;
using System.ComponentModel;
using FluentAssertions;
using NinjaTools.FluentMockServer.API.Extensions;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.API.Tests.Extensions
{
    public class EnumExtensions : XUnitTestBase<EnumExtensions>
    {
        /// <inheritdoc />
        public EnumExtensions(ITestOutputHelper output) : base(output)
        {
        }

        public enum Animals
        {
            Dog,
            Cat,
            Fish
        }


        private const string DogDescription = "The best friend of the human.";

        public enum AnimalsWithDescription
        {
            [Description(DogDescription)]
            Dog,
            [Description()]
            Cat,
            Fish
        }

        [Theory]
        [InlineData(Animals.Cat)]
        [InlineData(Animals.Dog)]
        [InlineData(Animals.Fish)]
        [InlineData(AnimalsWithDescription.Fish)]
        public void GetDescription_Should_Throw_When_Enum_Does_Not_Contain_Description_Attribute<T>(T value) where T : Enum, IConvertible
        {
            // Act
            Func<string> invocation = () => value.GetDescription();

            // Assert
            invocation.Should().ThrowExactly<InvalidOperationException>();
        }



        [Fact]
        public void GetDescription_Should_Return_DescriptionValue_When_EnumValue_Contains_Description_Attribute()
        {
            // Arrange
            var value = AnimalsWithDescription.Dog;

            // Act & Assert
            value.GetDescription().Should().Be(DogDescription);
        }

        [Fact]
        public void GetDescription_Should_Return_DescriptionValue_When_Enum_Contains_Empty_Description_Attribute()
        {
            // Arrange
            var value = AnimalsWithDescription.Cat;

            // Act & Assert
            value.GetDescription().Should().Be(string.Empty);
        }

    }
}
