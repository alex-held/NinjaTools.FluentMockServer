using System.IO;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.FluentAPI.Builders;
using NinjaTools.FluentMockServer.FluentAPI.Builders.HttpEntities;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Serialization;
using Xunit;

namespace NinjaTools.FluentMockServer.Tests.Serialization
{
    
    public class ExpectationConverterTests
    {
        [Fact]
        public void Should_Use_ExpectationConverter_When_Using_Standard_Deserializer()
        {
            // Arrange
            const string json = @"{
  ""httpRequest"": {
    ""path"": ""/some/path""
  }
}";
            var builder = new FluentHttpRequestBuilder().WithPath("/some/path");
            
            // Act
            var unused =Verify.Once(builder.Build());
            var expected = FluentExpectationBuilder.Create(httpRequest:builder.Build());

            // Act
            var result = JsonConvert.DeserializeObject<Expectation>(json);

            // Assert
            result
                .Should()
                .BeOfType<Expectation>()
                .Which
                .HttpRequest.Path.Should().Be(expected.HttpRequest.Path);
        }

        [Fact]
        public void Should_Convert_To_Expectation_When_Converting_From_String()
        {
            // Arrange
            const string json = @"{
  ""httpRequest"": {
    ""path"": ""/some/path""
  },
 ""httpResponse"": {
    ""statusCode"": 201,
    ""delay"": {
      ""timeUnit"": ""MILLISECONDS"",
      ""value"": 1
    }
  },
  ""times"": {
    ""remainingTimes"": 1,
    ""unlimited"": false
  }
}";
            var builder = new FluentHttpRequestBuilder()
                .WithPath("/some/path");
            
            var expected = FluentExpectationBuilder.Create(
                httpRequest: builder.Build(),
                httpResponse:  HttpResponse.Create(
                    statusCode:201,
                    delay: new Delay(TimeUnit.Milliseconds, 1)),
                times: Times.Once
            );

            // Act
            var jsonReader = new JsonTextReader(new StringReader(json));
            var sut = new ExpectationConverter();
            var result = sut.ReadJson(jsonReader, typeof(Expectation), null, JsonSerializer.Create(Serializer.SerializerSettings)) as Expectation;

            // Assert
            result
                .Should()
                .BeOfType<Expectation>()
                .Which
                .HttpRequest.Path.Should().Be(expected.HttpRequest.Path);
        }

        [Fact]
        public void Should_Use_ExpectationConverter_When_Using_Standard_Serializer()
        {
            // Arrange
            var expected = JObject.Parse(@"{
  ""httpRequest"": {
    ""path"": ""/some/path""
  }
}").ToString(Formatting.Indented);

            var builder = new FluentHttpRequestBuilder()
                .WithPath("/some/path");
            var expectation = FluentExpectationBuilder.Create(httpRequest:builder.Build());

            // Act
            var json = JsonConvert.SerializeObject(expectation);

            // Assert
            json.Should().Be(expected);
        }


        [Fact]
        public void Should_Convert_Expectation_To_Json()
        {
            // Arrange
            var expected = JObject.Parse(@"{
  ""httpRequest"": {
    ""path"": ""/some/path""
  }
}").ToString(Formatting.Indented);

            var builder = new FluentHttpRequestBuilder()
                .WithPath("/some/path");
            var expectation = FluentExpectationBuilder.Create(httpRequest: builder.Build());

            var subject = CreateSubject(out var sb, out var writer);

            // Act
            subject.WriteJson(writer, expectation, JsonSerializer.CreateDefault());

            // Assert
            var json = sb.ToString();
            json.Should().Be(expected);
        }

        private static ExpectationConverter CreateSubject(out StringBuilder sb, out JsonWriter writer)
        {
            var sut = new ExpectationConverter();
            sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            writer = new JsonTextWriter(stringWriter);
            return sut;
        }
    }
}
