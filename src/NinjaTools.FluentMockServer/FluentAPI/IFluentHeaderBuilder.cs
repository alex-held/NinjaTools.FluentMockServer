using System.Collections.Generic;
using System.Net.Http.Headers;

namespace NinjaTools.FluentMockServer.FluentAPI
{
    public interface IFluentResponseHeaderBuilder : IFluentHeaderBuilder
    {
        ContentDispositionHeaderValue ContentDisposition { get; set; }
        IFluentHeaderBuilder WithContentDispositionHeader(string type, string name, string filename);
    }

    public interface IFluentHeaderBuilder : IFluentInterface
    {
        AuthenticationHeaderValue Authentication { get; set; }
        IFluentHeaderBuilder AddHeaders(params (string name, string value)[] headers);
        IFluentHeaderBuilder Add(string name, string value);
        IFluentHeaderBuilder AddContentType(string contentType);
        IFluentHeaderBuilder AddBasicAuth(string username, string password);
        Dictionary<string, string[]> Build();
    }
}
