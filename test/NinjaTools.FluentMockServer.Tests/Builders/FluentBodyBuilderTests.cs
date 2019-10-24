using System;
using System.Linq;

using FluentAssertions.Json;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NinjaTools.FluentMockServer.Builders;

using Xunit;
using Xunit.Abstractions;


namespace NinjaTools.FluentMockServer.Tests.Builders
{
    public partial class FluentBodyBuilderTests
    {
        private void Assert(string expected, Action<IFluentBodyBuilder> factory)
        {
            var builder = new  FluentBodyBuilder();
            factory(builder);
            
            var resultJToken = builder.Build().SerializeJObject();
            var expectedJToken = JObject.Parse(expected);
            
            var envelope = new JObject(new JProperty("body", "{CONTENT}"));

            var body = envelope.Children()
                        .OfType<JProperty>()
                        .First(jp => jp.Name == "body").Value
                        .Value<JValue>();
            
            body.Replace(resultJToken);
            
            var result = envelope.ToString(Formatting.Indented);
            _outputHelper.WriteLine($"Expected:\n\n{expected}\n");
            _outputHelper.WriteLine($"Actual:\n\n{result}\n");

            JsonAssertionExtensions.Should(envelope).BeEquivalentTo(expectedJToken);
            
        }
        private readonly ITestOutputHelper _outputHelper;
        public FluentBodyBuilderTests(ITestOutputHelper outputHelper) =>  _outputHelper = outputHelper;
        
        
        [Fact]
        public void WithExactJson()
        {
            // Arrange
            const string content = "{ \"id\": 1, \"name\": \"A green door\", \"price\": 12.50, \"tags\": [\"home\", \"green\"] }";
            const string expected = @"{
    ""body"": {
        ""type"": ""JSON"",
        ""json"": ""{ \""id\"": 1, \""name\"": \""A green door\"", \""price\"": 12.50, \""tags\"": [\""home\"", \""green\""] }"",
        ""matchType"": ""STRICT""
    }
}";     
            // Act & Assert
            Assert(expected, builder => builder.WithExactJson(content));
        }


        [Fact]
        public void ContainingJson()
        {
            // Arrange
            const string content = "{ \"id\": 1, \"name\": \"A green door\", \"price\": 12.50, \"tags\": [\"home\", \"green\"] }";
            const string expected = @"{
    ""body"": {
        ""type"": ""JSON"",
        ""json"" : ""{ \""id\"": 1, \""name\"": \""A green door\"", \""price\"": 12.50, \""tags\"": [\""home\"", \""green\""] }""
    }
}";
            // Act & Assert
            Assert(expected, builder => builder.ContainingJson(content));
        }

        [Fact]
        public void MatchingJsonSchema()
        {
            // Arrange
            const string content = @"{""$schema"": ""http://json-schema.org/draft-04/schema#"", ""title"": ""Product"", ""description"": ""A product from Acme's catalog"", ""type"": ""object"", ""properties"": { ""id"": { ""description"": ""The unique identifier for a product"", ""type"": ""integer"" }, ""name"": { ""description"": ""Name of the product"", ""type"": ""string""}, ""price"": { ""type"": ""number"", ""minimum"": 0, ""exclusiveMinimum"": true }, ""tags"": { ""type"": ""array"", ""items"": { ""type"": ""string"" }, ""minItems"": 1, ""uniqueItems"": true } }, ""required"": [""id"", ""name"", ""price""] }";
            const string expected = @"{
            ""body"": {
            ""type"": ""JSON_SCHEMA"",
            ""jsonSchema"" : ""{\""$schema\"": \""http://json-schema.org/draft-04/schema#\"", \""title\"": \""Product\"", \""description\"": \""A product from Acme's catalog\"", \""type\"": \""object\"", \""properties\"": { \""id\"": { \""description\"": \""The unique identifier for a product\"", \""type\"": \""integer\"" }, \""name\"": { \""description\"": \""Name of the product\"", \""type\"": \""string\""}, \""price\"": { \""type\"": \""number\"", \""minimum\"": 0, \""exclusiveMinimum\"": true }, \""tags\"": { \""type\"": \""array\"", \""items\"": { \""type\"": \""string\"" }, \""minItems\"": 1, \""uniqueItems\"": true } }, \""required\"": [\""id\"", \""name\"", \""price\""] }""    }
            }"; 
            
             const string expectedNegated = @"{
            ""body"": {
            ""not"": true,
            ""type"": ""JSON_SCHEMA"",
            ""jsonSchema"" : ""{\""$schema\"": \""http://json-schema.org/draft-04/schema#\"", \""title\"": \""Product\"", \""description\"": \""A product from Acme's catalog\"", \""type\"": \""object\"", \""properties\"": { \""id\"": { \""description\"": \""The unique identifier for a product\"", \""type\"": \""integer\"" }, \""name\"": { \""description\"": \""Name of the product\"", \""type\"": \""string\""}, \""price\"": { \""type\"": \""number\"", \""minimum\"": 0, \""exclusiveMinimum\"": true }, \""tags\"": { \""type\"": \""array\"", \""items\"": { \""type\"": \""string\"" }, \""minItems\"": 1, \""uniqueItems\"": true } }, \""required\"": [\""id\"", \""name\"", \""price\""] }""    }
            }"; 
 
            // Act & Assert
            Assert(expected, builder => builder.MatchingJsonSchema(content));
        }

        [Fact]
        public void MatchingJsonPath()
        {
            // Arrange
            const string content = "$.store.book[?(@.price < 10)]";
            const string expected = @"{
    ""body"": {
        ""type"": ""JSON_PATH"",
        ""jsonPath"": ""$.store.book[?(@.price < 10)]""
    }
}";
            
            // Act & Assert
            Assert(expected,builder => builder.MatchingJsonPath(content));
        }


        [Fact]
        public void MatchingXPath()
        {
            // Arrange
            const string content = "/bookstore/book[price>30]/price";
            const string expected = @"{
    ""body"": {
        ""type"": ""XPATH"",
        ""xpath"": ""/bookstore/book[price>30]/price""
    }
}";
            
            // Act & Assert
            Assert(expected, builder => builder.MatchingXPath(content));
        }

        [Fact]
        public void WithXmlContent()
        {
            // Arrange
            const string content =
                "<bookstore> <book nationality=\"ITALIAN\" category=\"COOKING\"><title lang=\"en\">Everyday Italian</title><author>Giada De Laurentiis</author><year>2005</year><price>30.00</price></book> </bookstore>";
            const string expected = @"{
    ""body"": {
        ""type"": ""XML"",
        ""xml"": ""<bookstore> <book nationality=\""ITALIAN\"" category=\""COOKING\""><title lang=\""en\"">Everyday Italian</title><author>Giada De Laurentiis</author><year>2005</year><price>30.00</price></book> </bookstore>""
    }
}";
            // Act & Assert
            Assert(expected, builder => builder.WithXmlContent(content));
        }


        [Fact]
        public void MatchingXmlSchema()
        {
            // Arrange
            const string content =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?> <xs:schema xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" elementFormDefault=\"qualified\" attributeFormDefault=\"unqualified\"> <xs:element name=\"notes\"> <xs:complexType> <xs:sequence> <xs:element name=\"note\" maxOccurs=\"unbounded\"> <xs:complexType> <xs:sequence> <xs:element name=\"to\" minOccurs=\"1\" maxOccurs=\"1\" type=\"xs:string\"></xs:element> <xs:element name=\"from\" minOccurs=\"1\" maxOccurs=\"1\" type=\"xs:string\"></xs:element> <xs:element name=\"heading\" minOccurs=\"1\" maxOccurs=\"1\" type=\"xs:string\"></xs:element> <xs:element name=\"body\" minOccurs=\"1\" maxOccurs=\"1\" type=\"xs:string\"></xs:element> </xs:sequence> </xs:complexType> </xs:element> </xs:sequence> </xs:complexType> </xs:element> </xs:schema>";
            const string expected = @"{
    ""body"": {
        ""type"": ""XML_SCHEMA"",
        ""xmlSchema"": ""<?xml version=\""1.0\"" encoding=\""UTF-8\""?> <xs:schema xmlns:xs=\""http://www.w3.org/2001/XMLSchema\"" elementFormDefault=\""qualified\"" attributeFormDefault=\""unqualified\""> <xs:element name=\""notes\""> <xs:complexType> <xs:sequence> <xs:element name=\""note\"" maxOccurs=\""unbounded\""> <xs:complexType> <xs:sequence> <xs:element name=\""to\"" minOccurs=\""1\"" maxOccurs=\""1\"" type=\""xs:string\""></xs:element> <xs:element name=\""from\"" minOccurs=\""1\"" maxOccurs=\""1\"" type=\""xs:string\""></xs:element> <xs:element name=\""heading\"" minOccurs=\""1\"" maxOccurs=\""1\"" type=\""xs:string\""></xs:element> <xs:element name=\""body\"" minOccurs=\""1\"" maxOccurs=\""1\"" type=\""xs:string\""></xs:element> </xs:sequence> </xs:complexType> </xs:element> </xs:sequence> </xs:complexType> </xs:element> </xs:schema>""
    }
}";
            // Act & Assert
            Assert(expected, builder => builder.MatchingXmlSchema(content));
        }


        [Fact]
        public void ContainingSubstring()
        {
            // Arrange
            const string content = "some_string";
            const string expected = @"{
    ""body"": {
        ""type"": ""STRING"",
        ""string"": ""some_string"",
        ""subString"": true
    }
}";
            // Act & Assert
            Assert(expected,  builder => builder.ContainingSubstring(content));
        }
        
        [Fact]
        public void WithExactString()
        {
            // Arrange
            const string content = "some_string";
            const string expected = @"{
    ""body"": {
        ""type"": ""STRING"",
        ""string"": ""some_string""
    }
}";
            // Act & Assert
            Assert(expected, builder => builder.WithExactString(content));
        }
        
        
        [Fact]
        public void WithExactStringAndContentType()
        {
            // Arrange
            const string content = "some_string";
            const string expected = @"{
    ""body"": {
        ""type"": ""STRING"",
        ""string"": ""some_string"",
        ""contentType"": ""application/json""
    }
}";
            // Act & Assert
            Assert(expected, builder => builder.WithExactString(content).WithContentType("application/json"));
        }
    }
}
