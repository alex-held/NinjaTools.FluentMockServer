using NinjaTools.FluentMockServer.FluentInterfaces;


namespace NinjaTools.FluentMockServer.Builders
{
    public interface IFluentHeaderBuilder : IFluentInterface
    {
        IFluentHeaderBuilder WithHeaders(params (string name, string value)[] headers);
        IFluentHeaderBuilder AddHeader(string name, string value);
    }
}