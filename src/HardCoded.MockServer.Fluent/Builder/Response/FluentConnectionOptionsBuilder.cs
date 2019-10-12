using System;
using HardCoded.MockServer.Models;

namespace HardCoded.MockServer.Fluent.Builder.Response
{
    internal class FluentConnectionOptionsBuilder : IFluentConnectionOptionsBuilder
    {
        private readonly ConnectionOptions _options;

        public FluentConnectionOptionsBuilder()
        {
            _options = new ConnectionOptions();
        }

        /// <inheritdoc />
        public ConnectionOptions Build() => _options;
    }
}