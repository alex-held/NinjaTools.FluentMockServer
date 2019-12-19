using System;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Extensions;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NinjaTools.FluentMockServer.Tests.Serialization
{
    public class ContentSerializationTests : XUnitTestBase<ContentSerializationTests>
    {
        public ContentSerializationTests(ITestOutputHelper outputHelper) : base(outputHelper)
        { }
        
        [Fact]
        public void Should_Serialize_Binary_Content()
        {
            // Arrange
            var httpResponse = HttpResponse.Create(
                body: new BinaryContent("iVBORw0KGgoAAAANSUhEUgAAAqwAAAApCAIAAAB"), 
                delay: new Delay(TimeUnit.Milliseconds, 50));
        
            // Act
            httpResponse.AsJObject();
            var json = httpResponse.AsJson();
            
            Output.WriteLine(json);

            const string expected = @"
{
    ""delay"": {
        ""timeUnit"": ""MILLISECONDS"",
        ""value"": 50
    },
    ""body"": {
        ""base64Bytes"": ""iVBORw0KGgoAAAANSUhEUgAAAqwAAAApCAIAAAB"",
        ""type"": ""BINARY""
    }
}";
            
            // Assert
            var expectedJObject = JObject.Parse(expected);
            var actualJObject = JObject.Parse(json);
            JToken.EqualityComparer.Equals(actualJObject, expectedJObject).Should().BeTrue();
        }






        public class JsonParserData : TheoryData<string, JToken>
        {
            
            private static readonly string S1 = @"
""httpResponse"": {
    ""body"": ""some_response_body""
}";

            private static readonly JObject J1 = new JObject(new JProperty("httpResponse", new JObject
            {
                ["body"] = "some_response_body"
            }));
            
            
            private static readonly string S2 = @"
""httpRequest"": {
 ""path"": ""/some/path""  
}";
            private static readonly JObject J2 = new JObject( new JProperty("httpRequest", new JObject
            {
                ["path"] = "/some/path"
            }));

            
            
            
            public JsonParserData()
            {
                AddRow(S1, J1);
                AddRow(S2, J2);
                AddRow(BinaryBodyJson, BinaryBodyJProperty);
                AddRow("\"body\": \"123\"", new JProperty("body", new JValue("123")));
              
            }
            
            
            
            
            public const string BinaryBodyJson = @"
""body"": {
    ""type"": ""BINARY"",
    ""base64Bytes"": ""iVBORw0KGgoAAAANSUhEUgAAAqwAAAApCAIAAAB/QuwlAAAK+GlDQ""
}";
            public static readonly JObject BinaryBodyJProperty = new JObject(new JProperty("body", new JObject
            {
                ["type"] = "BINARY",
                ["base64Bytes"] = "iVBORw0KGgoAAAANSUhEUgAAAqwAAAApCAIAAAB/QuwlAAAK+GlDQ"
            }));
        }

        [Theory]
        [ClassData(typeof(JsonParserData))]
        public void Should_Parse_Strings(string json, JToken jToken)
        {
            // Act
            if (!Parser.TryParsePartialJson(json, out var result, out var exceptionPath))
            {
                Output.WriteLine(exceptionPath);
                throw new XunitException($"Parsing the input {json} failed.");
            }
            
            
            Output.WriteLine(result.ToString(Formatting.Indented));
            result.ToString().Should().Be(jToken.ToString());
        }
    }

    public static class Parser
    {
        public static bool TryParsePartialJson(string json, out JToken token, out string exceptionPath)
        {
            token = null;
            exceptionPath = string.Empty;
            json = json.Trim();

            try
            {
                 
                // JObject
                if ((json.StartsWith("{") && json.EndsWith("}")))
                {
                    token = JToken.Parse(json);
                    return true;
                }
                // JArray
                //
                else if (json.StartsWith("[") && json.EndsWith("]"))
                {
                    token = JToken.Parse(json);
                    return true;
                }
                
                // JProperty
                if (json.IsContainerByEither('\"'))
                {
                    /*
                    var quotesCount = json.Count(c => c == '\'');
                    var doubleQuotesCount = json.Count(c => c == '"');
                    
                    if (quotesCount >= doubleQuotesCount)
                    {
                        json = json.Replace('\'', '\"');
                    }
                 */
   
                    var validJson = $"{{ {json} }}";
                    var isValid = TryResolveJProperty(out token, validJson);
                    return isValid;
                } 
                
                /* 
                 * JObject with properties
                 * @"{
    ""body"" : {
        ""type"" : ""BINARY"",
        ""base64Bytes"" : ""iVBORw0KGgoAAAANSUhEUgAAAqwAAAApCAIAAAB/QuwlAAAKClHJFGbTS7HwlKuglogc1nrrrB3XO6kuSm""
    }}" */
                if (json.StartsWithEither('\"') &&  json.EndsWithEither('}'))
                {
                    var validJson = $"{{ {json} }}";
                    dynamic jo = JsonConvert.DeserializeObject<JObject>(validJson);
                    token = jo;
                    return true;
                }
                
                // JProperty
                else if (json.StartsWith("\"") && json.EndsWith("\""))
                {
                    var validJson = $"{{ {json} }}";
                    return TryResolveJProperty(out token, validJson);
                }
                else
                {
                    var validJson = $"{{ {json} }}";
                    dynamic jo = JsonConvert.DeserializeObject<JObject>(validJson);
                    token = jo;
                    return true;
                }
            }
            catch (JsonReaderException readerException)
            {
                exceptionPath = readerException.Path;
                Debug.WriteLine(readerException);
                return false;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                return false;
            }
        }

        private static bool TryResolveJProperty(out JToken token, string validJson)
        {
            if (IsValidJson(validJson))
            {
                var jObject = JObject.Parse(validJson);
                var properties = jObject
                    .Properties()
                    ?.ToList();

                if (properties?.Count() > 1)
                {
                    token = jObject;
                    return true;
                }
                else if (properties?.Count() == 1)
                {
                    token = properties.First();
                    return true;
                }

                token = jObject;
                return true;
            }

            token = null;
            return false;
        }

        internal static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var unused = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
