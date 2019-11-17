using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Models;

[assembly: InternalsVisibleTo("NinjaTools.FluentMockServer.Tests")]
namespace NinjaTools.FluentMockServer.Builders.Request
{
    internal class FluentBodyBuilder : IFluentBodyBuilder
    {
        private RequestBody _body;
        
        public RequestBody Body
        {
            get => _body ??= new RequestBody(RequestBody.BodyType.STRING, false);
            set => _body = value;
        }
        
        private JToken Token { get; set; }


        private void Invert(Action action)
        {
            action();

            Body.Not = Body.Not.HasValue
                        ? ( bool? ) null
                        : true;
        }


        private T Invert<T>(Func<T> action)
        {
            var result = action();

            Body.Not = Body.Not.HasValue
                        ? ( bool? ) null
                        : true;

            return result;
        }
        
        /// <inheritdoc />
        public void WithBinary(string base64)
        {
            _body ??= new RequestBody(RequestBody.BodyType.BINARY, false);
            _body.Base64Bytes = base64;
        }

        /// <inheritdoc />
        public void WithLiteral(string literal)
        {
            Body = new RequestBody();
            Body.Literal = literal;
        }

        /// <inheritdoc />
        public void ContainingJson
                    (string subJson) => Body = RequestBody.MatchPartialJson(subJson);


        /// <inheritdoc />
        public void NotContainingJson
                    (string subJson) => Invert(() => ContainingJson(subJson));


        /// <inheritdoc />
        public void WithExactJson(string content) => Body = RequestBody.MatchExactJson(content);


        /// <inheritdoc />
        public void WithoutExactJson(string json) => Invert(() => WithExactJson(json));


        /// <inheritdoc />
        public void WithoutXmlContent(string content) => Invert(() => WithXmlContent(content));


        /// <inheritdoc />
        public void MatchingXPath(string path) => Body = RequestBody.MatchXPath(path);


        /// <inheritdoc />
        public void NotMatchingXPath(string path) => Invert(() => MatchingXPath(path));


        /// <inheritdoc />
        public void MatchingXmlSchema(string xmlSchema) => Body = RequestBody.MatchXmlSchema(xmlSchema);


        /// <inheritdoc />
        public void NotMatchingXmlSchema(string xmlSchema) => Invert(() => MatchingXmlSchema(xmlSchema));


        /// <inheritdoc />
        public void MatchingJsonPath(string path) => Body = RequestBody.MatchJsonPath(path);


        /// <inheritdoc />
        public void NotMatchingJsonPath(string path) => Invert(() => MatchingJsonPath(path));


        /// <inheritdoc />
        public void MatchingJsonSchema (string jsonSchema) => Body = RequestBody.MatchJsonSchema(jsonSchema);


        /// <inheritdoc />
        public void NotMatchingJsonSchema(string jsonSchema) => Invert(() => MatchingJsonSchema(jsonSchema));


        /// <inheritdoc />
        public void ContainingSubstring(string substring) => Body = RequestBody.MatchSubstring(substring);


        /// <inheritdoc />
        public void NotContainingSubstring(string substring) => Invert(() => ContainingSubstring(substring));

        /// <inheritdoc />
        public ISetupContentType WithExactString(string content)
        {
            Body = RequestBody.MatchString(content);
           return this;
        }

        /// <inheritdoc />
        public ISetupContentType WithoutExactString(string content) => Invert(() => WithExactString(content));
        
        /// <inheritdoc />
        public void WithXmlContent(string content) => Body = RequestBody.MatchXml(content);

        /// <inheritdoc />
        public void WithExactJsonForItem<T>
                    (T item) => Body = RequestBody.MatchPartialJson(
            JsonConvert.SerializeObject(item, Formatting.Indented));


        /// <inheritdoc />
        public void WithExactJsonForItems<T>
                    (params T[] items) => Body = RequestBody.MatchExactJson(
            JsonConvert.SerializeObject(items, Formatting.Indented));

        /// <inheritdoc />
        public void WithContentType(string contentType) => Body.ContentType = contentType;
        
        /// <inheritdoc />
        public void WithCommonContentType(CommonContentType contentType)
            => WithContentType(contentType.ToString());
        
        /// <inheritdoc />
        public JToken Build() => Token;
    }
}
