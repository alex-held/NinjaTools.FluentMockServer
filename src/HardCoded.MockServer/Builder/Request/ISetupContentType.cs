using System.Net.Mime;

using HardCoded.MockServer.Contracts;
using HardCoded.MockServer.Contracts.FluentInterfaces;


namespace HardCoded.MockServer.Builder.Request
{
    public interface ISetupContentType : IFluentInterface
    {
        void WithoutContentType();
        void WithContentType(string contentType);
        void WithContentType(ContentType contentType);
        void WithContentType(CommonContentType contentType);
    }
}