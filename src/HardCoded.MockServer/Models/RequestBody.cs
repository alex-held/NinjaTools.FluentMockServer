using HardCoded.MockServer.Contracts.Abstractions;
using HardCoded.MockServer.Models.HttpBodies;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace HardCoded.MockServer.Models
{
    public class RequestBody : IBuildable
    {
        /// <inheritdoc />
        public JObject Serialize() => JObject.FromObject(this);
        
        public RequestBody(BodyType type, bool invert)
        {
            Type = type;
            
            if (invert)
                Not = true;
        }

        public static RequestBody MatchExactJson(string json, bool invert = false)
        {
            return new RequestBody(BodyType.JSON, invert)
            {
                MatchType = Models.HttpBodies.MatchType.STRICT, 
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
        
        [JsonProperty("not", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Not { get; set; }
        
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public BodyType? Type { get; set; }
        
        [JsonProperty("string", NullValueHandling = NullValueHandling.Ignore)]
        public string? String { get; set; }
        
        [JsonProperty("subString", NullValueHandling = NullValueHandling.Ignore)]
        public bool? SubString { get; set; }
        
        [JsonProperty("xpath", NullValueHandling = NullValueHandling.Ignore)]
        public string? XPath { get; set; }
        
        [JsonProperty("xmlSchema", NullValueHandling = NullValueHandling.Ignore)]
        public string? XmlSchema { get; set; }
        
        [JsonProperty("json", NullValueHandling = NullValueHandling.Ignore)]
        public string? Json { get; set; }
        
        [JsonProperty("matchType", NullValueHandling = NullValueHandling.Ignore)]
        public MatchType? MatchType { get; set; }
        
        [JsonProperty("jsonSchema", NullValueHandling = NullValueHandling.Ignore)]
        public string? JsonSchema { get; set; }
        
        [JsonProperty("jsonPath", NullValueHandling = NullValueHandling.Ignore)]
        public string? JsonPath { get; set; }
        
        [JsonProperty("contentType", NullValueHandling = NullValueHandling.Ignore)]
        public string? ContentType { get; set; }
        
        [JsonProperty("xml", NullValueHandling = NullValueHandling.Ignore)]
        public string? Xml { get; set; }
    }
}