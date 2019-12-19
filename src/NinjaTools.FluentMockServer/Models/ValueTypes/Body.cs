using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    public partial class  Body
    {
        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is Body other))
            {
                return false;
            }
            
            if (!Children().Any() && !other.Children().Any())
            {
                return true;
            }

            var equals = JToken.DeepEquals(this, other);
            return equals;
        }        

        /// <inheritdoc />
        public override int GetHashCode()
        {
           var hashCode =  new JTokenEqualityComparer().GetHashCode(this);
           return hashCode;
        }
    }
    [Serializable]
    public partial class Body : JObject
    {
     

        /// <inheritdoc />
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

        internal Body AddOrUpdate(string key, string value)
        {
            return AddOrUpdate(key, new JValue(value));
        }

        public static Body Init(string type)
        {
            var body = new Body {{"type", new JValue(type)}};
            return body;
        }

        public Body AddOrUpdate(string key, JToken token)
        {
            if (TryGetValue(key, out var value)) value.Replace(token);
            Add(key, token);
            return this;
        }
    }
}
