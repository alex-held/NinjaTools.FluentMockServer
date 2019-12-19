using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Serialization;

namespace NinjaTools.FluentMockServer.FluentAPI
{
    public interface IFluentBodyBuilder : IFluentBuilder<JToken>
    {
        void WithBinary(string base64);

        void ContainingJson(string json);
        void NotContainingJson(string subJson);


        void WithExactJson(string content);
        void WithoutExactJson(string json);


        void WithXmlContent(string content);
        void WithoutXmlContent(string content);


        void MatchingXPath(string path);
        void NotMatchingXPath(string path);


        void MatchingXmlSchema(string xmlSchema);
        void NotMatchingXmlSchema(string xmlSchema);


        void MatchingJsonPath(string path);
        void NotMatchingJsonPath(string path);


        void MatchingJsonSchema(string jsonSchema);
        void NotMatchingJsonSchema(string jsonSchema);


        void ContainingSubstring(string substring);
        void NotContainingSubstring(string substring);


        void WithExactContent(string content, string contentType = null);
        void WithoutExactContent(string content, string contentType = null);


        void WithContentType(string contentType);
        void WithCommonContentType(CommonContentType contentType);


        void WithExactJsonForItems<T>(params T[] content);
        void WithExactJsonForItem<T>(T content);
    }
}
