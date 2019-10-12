using System;
using HardCoded.MockServer.Fluent.Builder.Request;
using HardCoded.MockServer.Tests.Utils;
using Xunit;
using Xunit.Abstractions;
using static HardCoded.MockServer.Tests.Utils.AssertionHelpers<HardCoded.MockServer.Fluent.Builder.Request.FluentBodyBuilder, HardCoded.MockServer.Fluent.Builder.Request.IFluentBodyBuilder, HardCoded.MockServer.Models.RequestBody>;

namespace HardCoded.MockServer.Fluent.Tests.Builders
{
    public class FluentBodyBuilderTests
    {
        private readonly ITestOutputHelper _outputHelper;
        public FluentBodyBuilderTests(ITestOutputHelper outputHelper) =>  _outputHelper = outputHelper;

        private void Assert(string expected, bool invertMatch, Action<IFluentBodyBuilder> factory, Action<IFluentBodyBuilder> invertedFactory)
        {
            Assert<BodyContainer>(_outputHelper, expected, invertMatch, factory, invertedFactory);
        }
        
        [Theory]
        [InvertMatch]
        public void With_Json_Exactly(bool invertMatch)
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
            Assert(expected, invertMatch, builder => builder.WithExactJson(content), builder => builder.WithoutExactJson(content));
        }

        [Theory]
        [InvertMatch]
        public void With_Json_Ignoring_Extra_Fields(bool invertMatch)
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
            Assert<BodyContainer>(_outputHelper, expected, invertMatch, builder => builder.ContainingJson(content), builder => builder.NotContainingJson(content));
        }

        [Theory]
        [InvertMatch]
        public void With_JsonSchema(bool invertMatch)
        {
            // Arrange
            const string content = @"{""$schema"": ""http://json-schema.org/draft-04/schema#"", ""title"": ""Product"", ""description"": ""A product from Acme's catalog"", ""type"": ""object"", ""properties"": { ""id"": { ""description"": ""The unique identifier for a product"", ""type"": ""integer"" }, ""name"": { ""description"": ""Name of the product"", ""type"": ""string""}, ""price"": { ""type"": ""number"", ""minimum"": 0, ""exclusiveMinimum"": true }, ""tags"": { ""type"": ""array"", ""items"": { ""type"": ""string"" }, ""minItems"": 1, ""uniqueItems"": true } }, ""required"": [""id"", ""name"", ""price""] }";
            const string expected = @"{
            ""body"": {
            ""type"": ""JSON_SCHEMA"",
            ""jsonSchema"" : ""{\""$schema\"": \""http://json-schema.org/draft-04/schema#\"", \""title\"": \""Product\"", \""description\"": \""A product from Acme's catalog\"", \""type\"": \""object\"", \""properties\"": { \""id\"": { \""description\"": \""The unique identifier for a product\"", \""type\"": \""integer\"" }, \""name\"": { \""description\"": \""Name of the product\"", \""type\"": \""string\""}, \""price\"": { \""type\"": \""number\"", \""minimum\"": 0, \""exclusiveMinimum\"": true }, \""tags\"": { \""type\"": \""array\"", \""items\"": { \""type\"": \""string\"" }, \""minItems\"": 1, \""uniqueItems\"": true } }, \""required\"": [\""id\"", \""name\"", \""price\""] }""    }
            }"; 
 
            // Act & Assert
            Assert(expected, invertMatch,builder => builder.MatchingJsonSchema(content), builder => builder.NotMatchingJsonSchema(content));
        }

        [Theory]
        [InvertMatch]
        public void With_JsonPath(bool invertMatch)
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
            Assert(expected,
                   invertMatch,
                   builder => builder.MatchingJsonPath(content),
                   builder => builder.NotMatchingJsonPath(content)
            );
        }


        [Theory]
        [InvertMatch]
        public void With_XPath(bool invertMatch)
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
            Assert(expected,
                   invertMatch,
                   builder => builder.MatchingXmlPath(content),
                   builder => builder.NotMatchingXmlPath(content)
            );
        }

        [Theory]
        [InvertMatch]
        public void With_Xml(bool invertMatch)
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
            Assert(expected, invertMatch, builder => builder.WithXmlContent(content), builder => builder.WithoutXmlContent(content));
        }


        [Theory]
        [InvertMatch]
        public void With_XmlSchema(bool invertMatch)
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
            Assert(expected, invertMatch, builder => builder.MatchingXmlSchema(content), builder => builder.NotMatchingXmlSchema(content));
        }


        [Theory]
        [InvertMatch]
        public void With_Substring(bool invertMatch)
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
            Assert(expected, invertMatch, builder => builder.ContainingSubstring(content), builder => builder.NotContainingSubstring(content));
        }
        
        [Theory]
        [InvertMatch]
        public void With_String(bool invertMatch)
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
            Assert(expected, invertMatch, 
                   builder => builder.WithExactString(content),
                   builder => builder.WithoutExactString(content));
        }
        
        
        [Theory]
        [InvertMatch]
        public void With_String_And_Content_Type(bool invertMatch)
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
            Assert(expected, invertMatch, 
                   builder => builder.WithExactString(content),
                   builder => builder.WithoutExactString(content));
        }
    }
}