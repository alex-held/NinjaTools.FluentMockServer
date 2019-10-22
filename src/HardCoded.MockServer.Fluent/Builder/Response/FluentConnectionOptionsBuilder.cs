using HardCoded.MockServer.Contracts.Models;


namespace HardCoded.MockServer.Fluent.Builder.Response
{
    internal class FluentConnectionOptionsBuilder : IFluentConnectionOptionsBuilder
    {
        private readonly ConnectionOptions _options;

        public FluentConnectionOptionsBuilder()
        {
            _options = new ConnectionOptions();
        }


        public ConnectionOptions Build() => _options;
    }
}