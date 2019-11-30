using System.ComponentModel;
using NinjaTools.FluentMockServer.Client.Models.ValueTypes;
using NinjaTools.FluentMockServer.FluentInterfaces;

namespace NinjaTools.FluentMockServer.Builders.Response
{
    /// <summary>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentConnectionOptionsBuilder : IFluentBuilder<ConnectionOptions>
    {
        ConnectionOptions Build();
    }
}
