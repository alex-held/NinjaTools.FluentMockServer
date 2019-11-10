using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Abstractions;


namespace NinjaTools.FluentMockServer.Models
{
  
    
    public class RequestBody : BuildableBase, IEquatable<RequestBody>
    {
        public const string MatchType_STRICT = "STRICT";
        public const string MatchType_ONLY_MATCHING_FIELDS = " ONLY_MATCHING_FIELDS";

        
        /// <inheritdoc />
        public override JObject SerializeJObject()
        {
            if (IsLiteral)
            {
                var jObj = new JObject();
                jObj.Add("body", Literal);
                return jObj;
            }
            
            return base.SerializeJObject();
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


        #region Setters

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
        

        #endregion

        #region Equality Members

        /// <inheritdoc />
        public bool Equals(RequestBody other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Type == other.Type && Not == other.Not && string.Equals(String, other.String, StringComparison.InvariantCultureIgnoreCase) && SubString == other.SubString && string.Equals(XPath, other.XPath, StringComparison.InvariantCultureIgnoreCase) && string.Equals(XmlSchema, other.XmlSchema, StringComparison.InvariantCultureIgnoreCase) && string.Equals(Json, other.Json, StringComparison.InvariantCultureIgnoreCase) && string.Equals(MatchType, other.MatchType, StringComparison.InvariantCultureIgnoreCase) && string.Equals(JsonSchema, other.JsonSchema, StringComparison.InvariantCultureIgnoreCase) && string.Equals(JsonPath, other.JsonPath, StringComparison.InvariantCultureIgnoreCase) && string.Equals(ContentType, other.ContentType, StringComparison.InvariantCultureIgnoreCase) && string.Equals(Xml, other.Xml, StringComparison.InvariantCultureIgnoreCase);
        }


        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals(( RequestBody ) obj);
        }


        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked {
                var hashCode = Type.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ Not.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ ( String != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(String) : 0 );
                hashCode = ( hashCode * 397 ) ^ SubString.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ ( XPath       != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(XPath) : 0 );
                hashCode = ( hashCode * 397 ) ^ ( XmlSchema   != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(XmlSchema) : 0 );
                hashCode = ( hashCode * 397 ) ^ ( Json        != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Json) : 0 );
                hashCode = ( hashCode * 397 ) ^ ( MatchType   != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(MatchType) : 0 );
                hashCode = ( hashCode * 397 ) ^ ( JsonSchema  != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(JsonSchema) : 0 );
                hashCode = ( hashCode * 397 ) ^ ( JsonPath    != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(JsonPath) : 0 );
                hashCode = ( hashCode * 397 ) ^ ( ContentType != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(ContentType) : 0 );
                hashCode = ( hashCode * 397 ) ^ ( Xml         != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(Xml) : 0 );

                return hashCode;
            }
        }


        public static bool operator ==(RequestBody left, RequestBody right) => Equals(left, right);


        public static bool operator !=(RequestBody left, RequestBody right) => !Equals(left, right);

        #endregion
    }
}
