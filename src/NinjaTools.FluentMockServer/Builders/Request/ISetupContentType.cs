using NinjaTools.FluentMockServer.FluentInterfaces;

namespace NinjaTools.FluentMockServer.Builders.Request
{
    public interface ISetupContentType : IFluentInterface
    { 
        void WithContentType(string contentType);
        void WithCommonContentType(CommonContentType contentType);
    }
}
