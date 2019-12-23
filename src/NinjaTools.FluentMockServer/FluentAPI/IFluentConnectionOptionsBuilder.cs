using System.ComponentModel;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.FluentAPI
{
    /// <summary>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentConnectionOptionsBuilder : IFluentBuilder<ConnectionOptions>
    {
        IFluentConnectionOptionsBuilder WithKeepAliveOverride(bool keepAliveOverride);
        IFluentConnectionOptionsBuilder WithCloseSocket(bool closeSocket);
        IFluentConnectionOptionsBuilder WithSuppressContentLengthHeader(bool suppressContentLengthHeader);
        IFluentConnectionOptionsBuilder WithSuppressConnectionHeader(bool suppressConnectionHeader);
        IFluentConnectionOptionsBuilder WithContentLengthHeaderOverride(long contentLengthHeaderOverride);
    }
}
