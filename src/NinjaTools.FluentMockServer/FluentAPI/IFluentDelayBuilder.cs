using System.ComponentModel;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.FluentAPI
{
    /// <summary>
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
