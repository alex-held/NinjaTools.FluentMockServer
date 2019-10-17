using HardCoded.MockServer.Contracts.FluentInterfaces;

using Newtonsoft.Json.Linq;


namespace HardCoded.MockServer.Contracts.Abstractions 
{
    public interface IBuildable : IFluentInterface
    {
        string Serialize();
        
        JObject SerializeJObject();
    }
}
