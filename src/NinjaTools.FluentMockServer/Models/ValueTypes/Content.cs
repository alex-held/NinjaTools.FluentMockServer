using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    [JsonObject(IsReference = true)]
    public class LiteralContent : Content
    {
        public LiteralContent(string literal)
        {
            Literal = literal ?? throw new ArgumentNullException(nameof(literal));
        }

        [JsonIgnore]
        public string Literal { get; set; }

        /// <inheritdoc />
        public override JToken Resolve()
        {
            var prop = new JProperty("body", Literal);
            return prop;
        }

        /// <inheritdoc />
        [JsonIgnore]
        public override string Type => null;
    }

    [JsonObject(IsReference = true)]
    public class BinaryContent : Content
    {
        /// <inheritdoc />
        public BinaryContent(byte[] bytes) : this(Convert.ToBase64String(bytes))
        {
        }

        public BinaryContent(string base64)
        {
            Base64Bytes = base64 ?? throw new ArgumentNullException(nameof(base64));
        }

        /// <inheritdoc />
        public override JToken Resolve()
        {
            var self = new JObject();
            self.Add("type", Type);
            self.Add("base64Bytes", Base64Bytes);
            var prop = new JProperty("body", self);
            return prop;
        }

        public string Base64Bytes { get; set; }

        /// <inheritdoc />
        public override string Type => "BINARY";
    }


    /*

        [JsonObject(MemberSerialization.OptOut, ItemNullValueHandling = NullValueHandling.Ignore, IsReference = false,
            NamingStrategyType = typeof(CamelCaseNamingStrategy))]*/
    [JsonObject(IsReference = true)]
    public abstract class Content
    {
        public abstract JToken Resolve();

        public abstract string Type { get; }
    }
}
