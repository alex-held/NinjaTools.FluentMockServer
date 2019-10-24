using System.Net.Mime;

using NinjaTools.FluentMockServer.FluentInterfaces;


namespace NinjaTools.FluentMockServer.Builders
{
    public interface ISetupContentType : IFluentInterface
    {
        void WithoutContentType();
        void WithContentType(string contentType);
        void WithContentType(ContentType contentType);
        void WithContentType(CommonContentType contentType);
    }
}