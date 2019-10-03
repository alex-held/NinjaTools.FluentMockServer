using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HardCoded.MockServer.Tests")]
namespace HardCoded.MockServer.Models.HttpBodies
{
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
        
        public RequestBody FromBinary(byte[] content)
        {
            var body = GetForType(BodyType.BINARY);
            body.Add("binary", content);
            return body;
        }

        private static RequestBody GetForType(BodyType type) => new RequestBody().SetType(type);
            
        public RequestBody FromJson(string content, MatchType? matchType = null)
        {
            var body = GetForType(BodyType.JSON);
            body.Add("json", content);
            if (matchType.HasValue)
                body.Add("matchType", matchType.Value.ToString().ToUpper());
            
            return body;
        }
        
        public RequestBody FromXml(string content)
        {
            var body = GetForType(BodyType.XML_SCHEMA);
            body.Add("xml", content);
            return body;
        }
            
        internal RequestBody SetType(BodyType type)
        {
            this["type"] = type.ToString().ToUpper();
            return this;
        }
    }
}