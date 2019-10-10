using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HardCoded.MockServer.Models
{
   

    public class Body : Dictionary<string, object>
    {
        public static Body For(BodyType type) => new Body(type);

        public Body()
        {
        }
                
        private Body(BodyType type)
        {
            SetBodyType(type); 
        }

        private void SetBodyType(BodyType bodyType)
        {
            Add("type", bodyType.ToString().ToUpper());
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
    }

}