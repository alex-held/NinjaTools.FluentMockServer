using System;
using System.Net.Http;
using HardCoded.MockServer.HttpBodies;

namespace HardCoded.MockServer.Builders
{
    
    public class RequestBuilder
    {
        private readonly IBodyBuilder _bodyBuilder;
        
        public RequestBuilder UsingMethod(HttpMethod method)
        {
            HttpMethod = method.ToString().ToUpper();
            return this;
        }
        
        public RequestBuilder WithPath(string path)
        {
            Path = path;
            return this;
        }

        public RequestBuilder WithBody(Action<IBodyBuilder> body)
        {
            body(_bodyBuilder);
            return this;
        }
        
    
        
        protected string HttpMethod { get; set; }
        protected string Path { get; set; }
        
    }

    public interface IBodyBuilder
    {
    }
}