using Xunit;

namespace HardCoded.MockServer.Fluent.Tests.Builders
{
    public partial class FluentBodyBuilderTests
    {
        
        [Fact]
        public void WithoutExactJson()
        {
            // Arrange
            const string content = "{ \"id\": 1, \"name\": \"A green door\", \"price\": 12.50, \"tags\": [\"home\", \"green\"] }";
            
            const string expected = @"{
    ""body"": {
        ""not"": true,
        ""type"": ""JSON"",
        ""json"": ""{ \""id\"": 1, \""name\"": \""A green door\"", \""price\"": 12.50, \""tags\"": [\""home\"", \""green\""] }"",
        ""matchType"": ""STRICT""
    }
}";
            Assert(expected, builder => builder.WithoutExactJson(content));
        }


        [Fact]
        public void NotContainingJson()
        {
            // Arrange
            const string content = "{ \"id\": 1, \"name\": \"A green door\", \"price\": 12.50, \"tags\": [\"home\", \"green\"] }";
            
            const string expected = @"{
    ""body"": {
        ""not"": true,
        ""type"": ""JSON"",
        ""json"" : ""{ \""id\"": 1, \""name\"": \""A green door\"", \""price\"": 12.50, \""tags\"": [\""home\"", \""green\""] }""
    }
}";
            // Act & Assert
            Assert(expected, builder => builder.NotContainingJson(content));
        }

        [Fact]
        public void NotMatchingJsonSchema()
        {
            // Arrange
            const string content = @"{""$schema"": ""http://json-schema.org/draft-04/schema#"", ""title"": ""Product"", ""description"": ""A product from Acme's catalog"", ""type"": ""object"", ""properties"": { ""id"": { ""description"": ""The unique identifier for a product"", ""type"": ""integer"" }, ""name"": { ""description"": ""Name of the product"", ""type"": ""string""}, ""price"": { ""type"": ""number"", ""minimum"": 0, ""exclusiveMinimum"": true }, ""tags"": { ""type"": ""array"", ""items"": { ""type"": ""string"" }, ""minItems"": 1, ""uniqueItems"": true } }, ""required"": [""id"", ""name"", ""price""] }";
            const string expected = @"{
            ""body"": {
            ""not"": true,
            ""type"": ""JSON_SCHEMA"",
            ""jsonSchema"" : ""{\""$schema\"": \""http://json-schema.org/draft-04/schema#\"", \""title\"": \""Product\"", \""description\"": \""A product from Acme's catalog\"", \""type\"": \""object\"", \""properties\"": { \""id\"": { \""description\"": \""The unique identifier for a product\"", \""type\"": \""integer\"" }, \""name\"": { \""description\"": \""Name of the product\"", \""type\"": \""string\""}, \""price\"": { \""type\"": \""number\"", \""minimum\"": 0, \""exclusiveMinimum\"": true }, \""tags\"": { \""type\"": \""array\"", \""items\"": { \""type\"": \""string\"" }, \""minItems\"": 1, \""uniqueItems\"": true } }, \""required\"": [\""id\"", \""name\"", \""price\""] }""    }
            }"; 
 
            // Act & Assert
            Assert(expected, builder => builder.NotMatchingJsonSchema(content));
        }

        [Fact]
        public void NotMatchingJsonPath()
        {
            // Arrange
            const string content = "$.store.book[?(@.price < 10)]";
            const string expected = @"{
    ""body"": {
        ""not"": true,
        ""type"": ""JSON_PATH"",
        ""jsonPath"": ""$.store.book[?(@.price < 10)]""
    }
}";        
            
            // Act & Assert
            Assert(expected, builder => builder.NotMatchingJsonPath(content));
        }


        [Fact]
        public void NotMatchingXPath()
        {
            // Arrange
            const string content = "/bookstore/book[price>30]/price";
            const string expected = @"{
    ""body"": {
        ""not"": true,
        ""type"": ""XPATH"",
        ""xpath"": ""/bookstore/book[price>30]/price""
    }
}";

            // Act & Assert
            Assert(expected,builder => builder.NotMatchingXPath(content));
        }

        [Fact]
        public void WithoutXmlContent()
        {
            // Arrange
            const string content =
                "<bookstore> <book nationality=\"ITALIAN\" category=\"COOKING\"><title lang=\"en\">Everyday Italian</title><author>Giada De Laurentiis</author><year>2005</year><price>30.00</price></book> </bookstore>";
            const string expected = @"{
    ""body"": {
        ""not"": true,
        ""type"": ""XML"",
        ""xml"": ""<bookstore> <book nationality=\""ITALIAN\"" category=\""COOKING\""><title lang=\""en\"">Everyday Italian</title><author>Giada De Laurentiis</author><year>2005</year><price>30.00</price></book> </bookstore>""
    }
}";
            // Act & Assert
            Assert(expected,builder => builder.WithoutXmlContent(content));
        }


        [Fact]
        public void NotMatchingXmlSchema()
        {
            // Arrange
            const string content =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?> <xs:schema xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" elementFormDefault=\"qualified\" attributeFormDefault=\"unqualified\"> <xs:element name=\"notes\"> <xs:complexType> <xs:sequence> <xs:element name=\"note\" maxOccurs=\"unbounded\"> <xs:complexType> <xs:sequence> <xs:element name=\"to\" minOccurs=\"1\" maxOccurs=\"1\" type=\"xs:string\"></xs:element> <xs:element name=\"from\" minOccurs=\"1\" maxOccurs=\"1\" type=\"xs:string\"></xs:element> <xs:element name=\"heading\" minOccurs=\"1\" maxOccurs=\"1\" type=\"xs:string\"></xs:element> <xs:element name=\"body\" minOccurs=\"1\" maxOccurs=\"1\" type=\"xs:string\"></xs:element> </xs:sequence> </xs:complexType> </xs:element> </xs:sequence> </xs:complexType> </xs:element> </xs:schema>";
            const string expected = @"{
    ""body"": {
        ""not"": true,
        ""type"": ""XML_SCHEMA"",
        ""xmlSchema"": ""<?xml version=\""1.0\"" encoding=\""UTF-8\""?> <xs:schema xmlns:xs=\""http://www.w3.org/2001/XMLSchema\"" elementFormDefault=\""qualified\"" attributeFormDefault=\""unqualified\""> <xs:element name=\""notes\""> <xs:complexType> <xs:sequence> <xs:element name=\""note\"" maxOccurs=\""unbounded\""> <xs:complexType> <xs:sequence> <xs:element name=\""to\"" minOccurs=\""1\"" maxOccurs=\""1\"" type=\""xs:string\""></xs:element> <xs:element name=\""from\"" minOccurs=\""1\"" maxOccurs=\""1\"" type=\""xs:string\""></xs:element> <xs:element name=\""heading\"" minOccurs=\""1\"" maxOccurs=\""1\"" type=\""xs:string\""></xs:element> <xs:element name=\""body\"" minOccurs=\""1\"" maxOccurs=\""1\"" type=\""xs:string\""></xs:element> </xs:sequence> </xs:complexType> </xs:element> </xs:sequence> </xs:complexType> </xs:element> </xs:schema>""
    }
}";
            // Act & Assert
            Assert(expected, builder => builder.NotMatchingXmlSchema(content));
        }


        [Fact]
        public void NotContainingSubstring()
        {
            // Arrange
            const string content = "some_string";
            const string expected = @"{
    ""body"": {
        ""not"": true,
        ""type"": ""STRING"",
        ""string"": ""some_string"",
        ""subString"": true
    }
}";
            // Act & Assert
            Assert(expected, builder => builder.NotContainingSubstring(content));
        }
        
        [Fact]
        public void WithoutExactString()
        {
            // Arrange
            const string content = "some_string";
            const string expected = @"{
    ""body"": {
        ""not"": true,
        ""type"": ""STRING"",
        ""string"": ""some_string""
    }
}";
            // Act & Assert
            Assert(expected, builder => builder.WithoutExactString(content));
        }
        
        
        [Fact]
        public void Without_String_And_Content_Type()
        {
            // Arrange
            const string content = "some_string";
            const string expected = @"{
    ""body"": {
        ""not"": true,
        ""type"": ""STRING"",
        ""string"": ""some_string"",
        ""contentType"": ""text/xml""
    }
}";
            // Act & Assert
            Assert(expected,builder => builder.WithoutExactString(content).WithContentType("text/xml"));
        }
    }
}
