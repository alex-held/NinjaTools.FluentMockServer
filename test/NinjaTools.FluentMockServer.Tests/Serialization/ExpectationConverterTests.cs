using System.IO;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Serialization
{
    public abstract class TestBase
    {
        protected ILogger Logger { get; }
        
        protected TestBase(ITestOutputHelper output)
        {
            Logger = LoggerFactory.Create(b => b.AddXunit(output)).CreateLogger(GetType().Name);
        }    
    } 
    
    public class ExpectationConverterTests : TestBase
    {
        /// <inheritdoc />
        public ExpectationConverterTests(ITestOutputHelper output) : base(output)
        {
        }

  
        [Fact]
        public void Should_Use_ExpectationConverter_When_Using_Standard_Deserializer()
        {
            // Arrange
            const string json = @"{
  ""httpRequest"": {
    ""path"": ""/some/path""
  }
}";
            var expected = new Expectation
            {
                HttpRequest = new HttpRequest
                {
                    Path = "/some/path"
                }
            };
            
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
  }
}";
            var expected = new Expectation
            {
                HttpRequest = new HttpRequest
                {
                    Path = "/some/path"
                }
            };
            
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
            const string expected = @"{
  ""httpRequest"": {
    ""path"": ""/some/path""
  }
}";
            var expectation = new Expectation
            {
                HttpRequest = new HttpRequest
                {
                    Path = "/some/path"
                }
            };
            
            // Act
            var json = JsonConvert.SerializeObject(expectation);
    
            // Assert
            json.Should().Be(expected);
        }

        
        [Fact]
        public void Should_Convert_Expectation_To_Json()
        {
            // Arrange
            const string expected = @"{
  ""httpRequest"": {
    ""path"": ""/some/path""
  }
}";
            var expectation = new Expectation
            {
                HttpRequest = new HttpRequest
                {
                    Path = "/some/path"
                }
            };
            
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
