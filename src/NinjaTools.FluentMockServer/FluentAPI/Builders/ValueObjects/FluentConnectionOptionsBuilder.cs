using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.FluentAPI.Builders.ValueObjects
{
    internal class FluentConnectionOptionsBuilder : IFluentConnectionOptionsBuilder
    {
        private bool? _closeSocket;
        private long? _contentLengthHeaderOverride;
        private bool? _suppressContentLengthHeader;
        private bool? _suppressConnectionHeader;
        private bool? _keepAliveOverride;

        /// <inheritdoc />
        [NotNull]
        public IFluentConnectionOptionsBuilder WithKeepAliveOverride(bool keepAliveOverride)
        {
            _keepAliveOverride = keepAliveOverride;
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentConnectionOptionsBuilder WithCloseSocket(bool closeSocket)
        {
            _closeSocket = closeSocket;
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentConnectionOptionsBuilder WithSuppressContentLengthHeader(bool suppressContentLengthHeader)
        {
            _suppressContentLengthHeader = suppressContentLengthHeader;
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentConnectionOptionsBuilder WithSuppressConnectionHeader(bool suppressConnectionHeader)
        {
            _suppressConnectionHeader = suppressConnectionHeader;
            return this;
        }

        /// <inheritdoc />
        [NotNull]
        public IFluentConnectionOptionsBuilder WithContentLengthHeaderOverride(long contentLengthHeaderOverride)
        {
            _contentLengthHeaderOverride = contentLengthHeaderOverride;
            return this;
        }

        [NotNull]
        public ConnectionOptions Build()
        {
            return new ConnectionOptions(_closeSocket, _contentLengthHeaderOverride, _suppressContentLengthHeader, _suppressConnectionHeader, _keepAliveOverride);
        }
    }
}
