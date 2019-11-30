using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Builders.Response
{
    internal class FluentConnectionOptionsBuilder : IFluentConnectionOptionsBuilder
    {
        private readonly ConnectionOptions _options;

        public FluentConnectionOptionsBuilder()
        {
            _options = new ConnectionOptions();
        }


        public ConnectionOptions Build()
        {
            return _options;
        }
    }
}
