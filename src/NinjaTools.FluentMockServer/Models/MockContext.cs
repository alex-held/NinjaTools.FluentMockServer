using System.Collections.Generic;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Models.HttpEntities;

namespace NinjaTools.FluentMockServer.Models
{
    public class MockContext
    {
        public const string Key = "X-MockServer-Context";
        public string Context { get; }
        public MockContext(string context) => Context = context;

        [NotNull]
        public static implicit operator string[](MockContext context) => new []
        {
            context.Context
        };

        public HttpRequest Apply(HttpRequest? httpRequest)
        {
            httpRequest ??= new HttpRequest();
            httpRequest.Headers ??= new Dictionary<string, string[]>();
            httpRequest.Headers.TryAdd(Key, this);
            return httpRequest;
        }
    }
}
