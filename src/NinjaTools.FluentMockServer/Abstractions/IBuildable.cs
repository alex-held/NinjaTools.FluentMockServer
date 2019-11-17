using Newtonsoft.Json.Linq;

using NinjaTools.FluentMockServer.FluentInterfaces;


namespace NinjaTools.FluentMockServer.Abstractions 
{
    public interface IBuildable : IFluentInterface
    {
        JObject SerializeJObject();
    }
}
