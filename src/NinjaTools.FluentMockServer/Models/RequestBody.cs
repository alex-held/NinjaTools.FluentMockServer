using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Utils;


namespace NinjaTools.FluentMockServer.Models
{
    [JsonObject(IsReference = true)]
    public class RequestBody : IBuildable
    {
        public const string MatchType_STRICT = "STRICT";
        public const string MatchType_ONLY_MATCHING_FIELDS = " ONLY_MATCHING_FIELDS";


        /// <inheritdoc />
        public JObject SerializeJObject()
        {

            return JObject.FromObject(this, Serializer.Default);

            var self = JObject.FromObject(this, Serializer.Default);

            // TODO: Fix this
            if (!IsLiteral)
            {
                return self;
            }

            var jObj = new JObject
            {
                {"body", Literal}
            };
            return jObj;

        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum BodyType
        {
            JSON_SCHEMA,
            JSON_PATH,
            JSON,
            STRING,
            XML_SCHEMA,
            XML,
            XPATH,
            REGEX,
            BINARY
        }

        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            if (IsLiteral)
            {
                Type = null;
                Not = null;
                String = null;
                SubString = null;
                XPath = null;
                XmlSchema = null;
                MatchType = null;
                JsonSchema = null;
                JsonPath = null;
                ContentType = null;
                Xml = null;
            }

        }

        [JsonIgnore]
        internal bool IsLiteral => !string.IsNullOrWhiteSpace(Literal);

        [JsonIgnore]
        public string Literal { get; set; }

        public BodyType? Type { get; set; }

        public bool? Not { get; set; }

        public string? String { get; set; }

        public bool? SubString { get; set; }

        [JsonProperty("xpath")]
        public string? XPath { get; set; }

        public string? XmlSchema { get; set; }

        public string? Json { get; set; }

        public string? MatchType { get; set; }

        public string? JsonSchema { get; set; }

        public string? JsonPath { get; set; }

        public string? ContentType { get; set; }

        public string? Xml { get; set; }
        public string? Base64Bytes { get; set; }

        public RequestBody()
        {
        }

        public RequestBody(BodyType type, bool invert)
        {
            Type = type;

            if (invert)
            {
                Not = true;
            }
        }

        public static RequestBody MatchExactJson(string json, bool invert = false)
        {
            return new RequestBody(BodyType.JSON, invert)
            {
                MatchType = MatchType_STRICT,
                Json = json
            };
        }

        public static RequestBody MatchPartialJson(string json, bool invert = false)
        {
            return new RequestBody(BodyType.JSON, invert)
            {
                Json = json
            };
        }

        public static RequestBody MatchJsonPath(string path, bool invert = false)
        {
            return new RequestBody(BodyType.JSON_PATH, invert)
            {
                JsonPath = path
            };
        }

        public static RequestBody MatchJsonSchema(string schema, bool invert = false)
        {
            return new RequestBody(BodyType.JSON_SCHEMA, invert)
            {
                JsonSchema = schema
            };
        }


        public static RequestBody MatchXmlSchema(string schema, bool invert = false)
        {
            return new RequestBody(BodyType.XML_SCHEMA, invert)
            {
                XmlSchema = schema
            };
        }

        public static RequestBody MatchXml(string xml, bool invert = false)
        {
            return new RequestBody(BodyType.XML, invert)
            {
                Xml = xml
            };
        }


        public static RequestBody MatchXPath(string path, bool invert = false)
        {
            return new RequestBody(BodyType.XPATH, invert)
            {
                XPath = path
            };
        }

        public static RequestBody MatchSubstring(string substring, string contentType = null, bool invert = false)
        {
            return new RequestBody(BodyType.STRING, invert)
            {
                String = substring,
                SubString = true,
                ContentType = contentType
            };
        }

        public static RequestBody MatchString(string @string, string contentType = null, bool invert = false)
        {
            return new RequestBody(BodyType.STRING, invert)
            {
                String = @string,
                ContentType = contentType
            };
        }


    }
}
