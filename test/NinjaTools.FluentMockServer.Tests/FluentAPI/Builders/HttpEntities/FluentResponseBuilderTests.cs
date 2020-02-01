using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.FluentAPI;
using NinjaTools.FluentMockServer.FluentAPI.Builders.HttpEntities;
using NinjaTools.FluentMockServer.Serialization;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.FluentAPI.Builders.HttpEntities
{
    public class FluentResponseBuilderTests : XUnitTestBase<FluentResponseBuilderTests>
    {

        /// <inheritdoc />
        public FluentResponseBuilderTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Should_Add_Header()
        {
            // Arrange
            var builder = new FluentHttpResponseBuilder();

            // Act
            var response = builder
                .ConfigureHeaders(opt => opt
                    .AddContentType("text/xml charset=UTF-8;"))
                .Build();

            // Assert
            response.Headers
                .GetValueOrDefault("Content-Type")
                .Should().ContainSingle("text/xml charset=UTF-8;")
                .And.HaveCount(1);
        }

        [Fact]
        public void Should_Add_Body_Literal()
        {
            // Arrange
            var builder = new FluentHttpResponseBuilder();

            // Act
            var response = Serializer.SerializeJObject(builder
                    .WithBody("Hello World!")
                    .Build());

            // Assert
            response.Value<string>("body").Should().Be("Hello World!");
        }



        [Fact]
        public void WithLiteralResponse_Creates_LiteralBody()
        {
            // Arrange
            var builder = new FluentHttpResponseBuilder();

            // Act
            var response =  Serializer.SerializeJObject(builder
                .WithBody("Hello World!")
                .Build());

            // Assert
            response.Value<string>("body").Should().Be("Hello World!");
        }

        [Fact]
        public void Should_Add_Multiple_Header_Values()
        {
            // Arrange
            IFluentHttpResponseBuilder builder = new FluentHttpResponseBuilder();

            // Act
            var response =  Serializer.SerializeJObject(builder
                .AddContentType("text/xml charset=UTF-8;")
                .WithHeader("Header-name 2", "true")
                .Build());


            Dump("JSON", response.ToString(Formatting.Indented));


            // Assert
            response["headers"]["Content-Type"][0].Value<string>().Should().Be("text/xml charset=UTF-8;");
            response["headers"]["Header-name 2"][0].Value<string>().Should().Be("true");
        }


    }
}
