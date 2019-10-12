using System.ComponentModel;
using HardCoded.MockServer.Contracts.FluentInterfaces;
using HardCoded.MockServer.Models.ValueTypes;

namespace HardCoded.MockServer.Fluent.Builder.Response
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentDelayBuilder : IFluentInterface
    {
        void FromSeconds(int seconds);
        void FromMiliSeconds(int ms);
        void FromMinutes(int minutes);
        Delay Build();
    }
}