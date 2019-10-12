using System;
using System.Net.Mime;
using HardCoded.MockServer.Contracts;
using HardCoded.MockServer.Models;
using HardCoded.MockServer.Models.HttpBodies;
using Newtonsoft.Json;

namespace HardCoded.MockServer.Fluent.Builder.Request
{
    internal class FluentBodyBuilder : IFluentBodyBuilder, ISetupContentType
    {

        private RequestBody _body;
        public RequestBody Body
        {
            get => _body ??= new RequestBody(BodyType.STRING, false);
            set => _body = value;
        }

        private void Invert(Action action)
        {
            action();
            
            Body.Not = Body.Not.HasValue 
                ? (bool?) null 
                : true;
        }
        
        private T Invert<T>(Func<T> action)
        {
            var result = action();
            
            Body.Not = Body.Not.HasValue 
                ? (bool?) null 
                : true;
            
            return result;
        }

        public ISetupContentType SetupContentType(Action action) =>
            new Func<ISetupContentType>(() =>
                {
                    action();
                    return this;
                }
            ).Invoke();



        /// <inheritdoc />
        public void ContainingJson(string subJson) => Body = RequestBody.MatchPartialJson(subJson);
        
        /// <inheritdoc />
        public void NotContainingJson(string subJson) =>  Invert(() => ContainingJson(subJson));
        
        /// <inheritdoc />
        public  void WithExactJson(string content) => Body = RequestBody.MatchExactJson(content);
        /// <inheritdoc />
        public void WithoutExactJson(string json) =>  Invert(() => WithExactJson(json));

        /// <inheritdoc />
        public void WithoutXmlContent(string content)  =>  Invert(() => WithXmlContent(content));
        
        /// <inheritdoc />
        public void MatchingXmlPath(string path) =>  Body = RequestBody.MatchXPath(path);   

        /// <inheritdoc />
        public void NotMatchingXmlPath(string path) =>  Invert( () => MatchingXmlPath(path));

        /// <inheritdoc />
        public void MatchingXmlSchema(string xmlSchema) => Body = RequestBody.MatchXmlSchema(xmlSchema);
       
        /// <inheritdoc />
        public void NotMatchingXmlSchema(string xmlSchema) =>  Invert(() => MatchingXmlSchema(xmlSchema));

        /// <inheritdoc />
        public void MatchingJsonPath(string path) =>     Body = RequestBody.MatchJsonPath(path);

        
        /// <inheritdoc />
        public void NotMatchingJsonPath(string path) =>             Invert(() =>   MatchingJsonPath(path));

        /// <inheritdoc />
        public void MatchingJsonSchema(string jsonSchema) =>  Body = RequestBody.MatchJsonSchema(jsonSchema);
        
        /// <inheritdoc />
        public void NotMatchingJsonSchema(string jsonSchema) => Invert( () =>  MatchingJsonSchema(jsonSchema));
    
        /// <inheritdoc />
        public void ContainingSubstring(string substring) =>     Body = RequestBody.MatchSubstring(substring);

        /// <inheritdoc />
        public void NotContainingSubstring(string substring) => Invert(() => ContainingSubstring(substring));
        
        /// <inheritdoc />
        public void WithXmlContent(string content) => Body = RequestBody.MatchXml(content);

        /// <inheritdoc />
        public void WithExactJsonForItem<T>(T item) => Body = RequestBody.MatchPartialJson(JsonConvert.SerializeObject(item, Formatting.Indented));
        
        /// <inheritdoc />
        public void WithExactJsonForItems<T>(params T[] items) => Body = RequestBody.MatchExactJson(JsonConvert.SerializeObject(items, Formatting.Indented));

        /// <inheritdoc />
        public ISetupContentType WithExactString(string content) => SetupContentType(() => Body = RequestBody.MatchString(content));

        /// <inheritdoc />
        public ISetupContentType WithoutExactString(string content) => Invert(() =>  WithExactString(content));

        /// <inheritdoc />
        public void WithoutContentType() => Body.ContentType = null;
        /// <inheritdoc />
        public void WithContentType(string contentType) =>  Body.ContentType = contentType;
        
        /// <inheritdoc />
        public void WithContentType(ContentType contentType) =>  WithContentType(contentType.ToString());
        
        /// <inheritdoc />
        public void WithContentType(CommonContentType contentType) =>   WithContentType(contentType.ToString());
        
        /// <inheritdoc />
        public RequestBody Build() => Body;
    }
}