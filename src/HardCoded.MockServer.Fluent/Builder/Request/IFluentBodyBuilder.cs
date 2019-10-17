using HardCoded.MockServer.Contracts.FluentInterfaces;
using HardCoded.MockServer.Models;

namespace HardCoded.MockServer.Fluent.Builder.Request
{
    public interface IFluentBodyBuilder : IFluentBuilder<RequestBody>, IFluentInterface
    {
        void ContainingJson(string subJson);
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
        
        
        ISetupContentType WithExactString(string content);
        ISetupContentType WithoutExactString(string content);
        
        
        void WithExactJsonForItems<T>(params T[] content);
        void WithExactJsonForItem<T>(T content);
    }
}