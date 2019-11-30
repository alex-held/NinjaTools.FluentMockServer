using System;
using Newtonsoft.Json.Linq;

namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    public class BinaryContent
    {
        public const string Type = "BINARY";

        /// <inheritdoc />
        public BinaryContent(byte[] bytes) : this(Convert.ToBase64String(bytes))
        {
        }

        public BinaryContent(string base64)
        {
            Base64Bytes = base64 ?? throw new ArgumentNullException(nameof(base64));
        }

        public string Base64Bytes { get; set; }

        public static implicit operator JToken(BinaryContent content)
        {
            return content.Resolve();
        }

        private JToken Resolve()
        {
            return new JObject {{"type", Type}, {"base64Bytes", Base64Bytes}};
        }
    }
}
