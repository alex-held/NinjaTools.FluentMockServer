using Newtonsoft.Json.Linq;

using NinjaTools.FluentMockServer.FluentInterfaces;


namespace NinjaTools.FluentMockServer.Abstractions 
{
    public interface IBuildable : IFluentInterface
    {
        string Serialize();
        
        JObject SerializeJObject();
    }
}
