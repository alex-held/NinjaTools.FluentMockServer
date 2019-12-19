using System.ComponentModel;
using NinjaTools.FluentMockServer.Domain.FluentInterfaces;
using NinjaTools.FluentMockServer.Domain.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Domain.Builders.Response
{
    /// <summary>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentConnectionOptionsBuilder : IFluentBuilder<ConnectionOptions>
    {
        IFluentConnectionOptionsBuilder WithKeepAliveOverride(bool keepAliveOverride);
        ConnectionOptions Build();
    }
}
