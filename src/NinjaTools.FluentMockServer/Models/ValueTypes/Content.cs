using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    public class BinaryContent 
    {
        public static implicit operator JToken(BinaryContent content)
        {
            return content.Resolve();
        }
        
          /// <inheritdoc />
        public BinaryContent(byte[] bytes) : this(Convert.ToBase64String(bytes))
        {
        }

        public BinaryContent(string base64)
        {
            Base64Bytes = base64 ?? throw new ArgumentNullException(nameof(base64));
        }

        private JToken Resolve()
        {
            var self = new JObject {{"type", Type}, {"base64Bytes", Base64Bytes}};
            var prop = new JProperty("body", self);
            return prop;
        }

        public string Base64Bytes { get; set; }

        
        public const string Type = "BINARY";
    }
}
