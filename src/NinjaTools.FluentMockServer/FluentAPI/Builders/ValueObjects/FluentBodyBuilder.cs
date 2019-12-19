using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Serialization;

namespace NinjaTools.FluentMockServer.FluentAPI.Builders.ValueObjects
{
    internal class FluentBodyBuilder : IFluentBodyBuilder
    {
        protected Body Body { get; set; } = new Body();

        /// <inheritdoc />
        public void WithBinary(string base64)
        {
            Add("BINARY", ("base64Bytes", base64));
        }

        /// <inheritdoc />
        public void ContainingJson(string json)
        {
            Add("JSON", ("json", json));
        }

        /// <inheritdoc />
        public void NotContainingJson(string json)
        {
            Invert(() => ContainingJson(json));
        }

        /// <inheritdoc />
        public void WithExactJson(string json)
        {
            Add("JSON", ("json", json), ("matchType", "STRICT"));
        }

        /// <inheritdoc />
        public void WithoutExactJson(string json)
        {
            Invert(() => WithExactJson(json));
        }


        /// <inheritdoc />
        public void WithoutXmlContent(string content)
        {
            Invert(() => WithXmlContent(content));
        }


        /// <inheritdoc />
        public void MatchingXPath(string path)
        {
            Add("XPATH", ("xpath", path));
        }


        /// <inheritdoc />
        public void NotMatchingXPath(string path)
        {
            Invert(() => MatchingXPath(path));
        }


        /// <inheritdoc />
        public void MatchingXmlSchema(string xmlSchema)
        {
            Add("XML_SCHEMA", ("xmlSchema", xmlSchema));
        }


        /// <inheritdoc />
        public void NotMatchingXmlSchema(string xmlSchema)
        {
            Invert(() => MatchingXmlSchema(xmlSchema));
        }


        /// <inheritdoc />
        public void MatchingJsonPath(string path)
        {
            Add("JSON_PATH", ("jsonPath", path));
        }


        /// <inheritdoc />
        public void NotMatchingJsonPath(string path)
        {
            Invert(() => MatchingJsonPath(path));
        }


        /// <inheritdoc />
        public void MatchingJsonSchema(string jsonSchema)
        {
            Add("JSON_SCHEMA", ("jsonSchema", jsonSchema));
        }

        /// <inheritdoc />
        public void NotMatchingJsonSchema(string jsonSchema)
        {
            Invert(() => MatchingJsonSchema(jsonSchema));
        }

        /// <inheritdoc />
        public void ContainingSubstring(string substring)
        {
            Add("STRING", ("string", substring), ("substring", true));
        }

        /// <inheritdoc />
        public void NotContainingSubstring(string substring)
        {
            Invert(() => ContainingSubstring(substring));
        }

        /// <inheritdoc />
        public void WithExactContent(string content, string contentType = null)
        {
            if (string.IsNullOrWhiteSpace(contentType))
            {
                Add("STRING", ("string", content));
                return;
            }

            Add("STRING", ("string", content), ("contentType", contentType));
        }

        /// <inheritdoc />
        public void WithoutExactContent(string content, string contentType = null)
        {
            Invert(() => WithExactContent(content, contentType));
        }

        /// <inheritdoc />
        public void WithXmlContent(string content)
        {
            Add("XML", ("xml", content));
        }

        /// <inheritdoc />
        public void WithExactJsonForItem<T>(T item)
        {
            WithExactJsonForItems(item);
        }

        /// <inheritdoc />
        public void WithExactJsonForItems<T>(params T[] items)
        {
            WithExactContent(JsonConvert.SerializeObject(items, Formatting.Indented), "application/json");
        }

        /// <inheritdoc />
        public void WithContentType(string contentType)
        {
            Body.AddOrUpdate("contentType", contentType);
        }

        /// <inheritdoc />
        public void WithCommonContentType(CommonContentType contentType)
        {
            WithContentType(contentType.ToString());
        }

        /// <inheritdoc />
        public JToken Build()
        {
            return Body;
        }

        private void Invert(Action action)
        {
            action();
            Body.AddOrUpdate("not", true);
        }


        internal void Add(string type, params (string key, object value)[] tuples)
        {
            Body = Body.Init(type);
            foreach (var (key, value) in tuples) Body.AddOrUpdate(key, new JValue(value));
        }
    }
}
