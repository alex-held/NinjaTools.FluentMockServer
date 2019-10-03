using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[assembly: InternalsVisibleTo("HardCoded.MockServer.Tests")]
namespace HardCoded.MockServer.HttpBodies
{
    
    public class RequestContext
    {
        
        [JsonProperty("body")]
        internal RequestBody Body { get; set; }
        
        public RequestContext WithBinary(byte[] content)
        {
            Body = RequestBody.FromBinary(content);
            return this;
        }
        
        public RequestContext WithJson(string content, MatchType? matchType = null)
        {
            Body = RequestBody.FromJson(content, matchType);
            return this;
        }
        
        public RequestContext WithXml(string content)
        {
            Body = RequestBody.FromXml(content);
            return this;
        }
    }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MatchType
    {
        STRICT,
        ONLY_MATCHING_FIELDS
    }
    
    public class BodyConverter : JsonConverter<Dictionary<string, object>>
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, Dictionary<string, object> value, JsonSerializer serializer)
        {
            writer.WriteValue(JsonConvert.SerializeObject(value, Formatting.Indented));
        }

        /// <inheritdoc />
        public override Dictionary<string, object> ReadJson(JsonReader reader, Type objectType, Dictionary<string, object> existingValue, bool hasExistingValue, JsonSerializer serializer) => throw new NotImplementedException();
    }
    
    [Newtonsoft.Json.JsonConverter(typeof(BodyConverter))]
    public class RequestBody : Dictionary<string, object>
    {
              
        public RequestBody MatchesXmlSchema(string xmlSchema)
        {
            var body = GetForType(BodyType.XML_SCHEMA);
            body.Add("xmlSchema", xmlSchema);
            return body;
        }
        
        
        public RequestBody MatchesXPath(string xpath)
        {
            var body = GetForType(BodyType.XPATH);
            body.Add("xpath", xpath);
            return body;
        }
        
        
        public RequestBody WithString(string content)
        {
            var body = GetForType(BodyType.STRING);
            body.Add("string", content);
            return body;
        }
        
        public RequestBody MatchesSubString(string content)
        {
            var body = GetForType(BodyType.STRING);
            body.Add("string", content);
            body.Add("subString", true);
            return body;
        }
        
        public bool? IsSubstring { get; set; }
        
      
      
        

        public RequestBody MatchesJsonSchema(string jsonSchema)
        {
            var body = GetForType(BodyType.JSON_SCHEMA);
            body.Add("jsonSchema", jsonSchema);
            return body;
        }
        
        
        
        public RequestBody MatchesRegex(string regex)
        {
            var body = GetForType(BodyType.REGEX);
            body.Add("regex", regex);
            return body;
        }

        public RequestBody MatchesJsonPath(string jsonPath)
        {
            var body = GetForType(BodyType.JSON_PATH);
            body.Add("jsonPath", jsonPath);
            return body;
        }
        
        /// <inheritdoc />
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);

        public static RequestBody FromBinary(byte[] content)
        {
            var body = GetForType(BodyType.BINARY);
            body.Add("binary", content);
            return body;
        }

        private static RequestBody GetForType(BodyType type) => new RequestBody().SetType(type);
            
        public static RequestBody FromJson(string content, MatchType? matchType = null)
        {
            var body = GetForType(BodyType.JSON);
            body.Add("json", content);
            if (matchType.HasValue)
                body.Add("matchType", matchType.Value.ToString().ToUpper());
            
            return body;
        }
        
        public static RequestBody FromXml(string content)
        {
            var body = GetForType(BodyType.XML_SCHEMA);
            body.Add("xml", content);
            return body;
        }
            
        public RequestBody SetType(BodyType type)
        {
            this["type"] = type.ToString().ToUpper();
            return this;
        }
    }
    
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
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
}