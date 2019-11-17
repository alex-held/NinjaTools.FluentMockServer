using System.ComponentModel;
using NinjaTools.FluentMockServer.FluentInterfaces;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Builders.Response
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentDelayBuilder : IFluentInterface
    {
        void FromSeconds(int seconds);
        void FromMilliSeconds(int ms);
        void FromMinutes(int minutes);
        Delay Build();
    }
}
