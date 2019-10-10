using System.ComponentModel;
using FluentApi.Generics.Framework;
using HardCoded.MockServer.Models.ValueTypes;

namespace HardCoded.MockServer.Fluent.Builder
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